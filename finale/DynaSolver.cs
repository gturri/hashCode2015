using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace finale
{
    public struct InstructionNode
    {
        public int Move;
        public int ParentIdx;
    }
    public struct ScoreAndInstruction
    {
        public int Score;
        public int InstructionIdx;
    }

	public class DynaSolver
	{
	    private readonly Problem _problem;
	    private readonly Solution _solution;
        private readonly Vec2[] _placedBalloonsPosition = new Vec2[53];
        private readonly int[] _placedBalloonsAltitude = new int[53];

	    private readonly InstructionNode[] tree = new InstructionNode[400*300*75*8];
	    private int _freeIdx;

	    public DynaSolver (Problem problem)
		{
		    _problem = problem;
            _solution = new Solution(problem);
		}

		public Solution Solve ()
		{
		    var sw = Stopwatch.StartNew();

		    for (int b = 0; b < 53; b++)
            {
                Console.WriteLine(b);

                //reset positions
                for (int i = 0; i < 53; i++)
                {
                    _placedBalloonsPosition[i] = _problem.DepartBallons;
                    _placedBalloonsAltitude[i] = 0;
                }
                MoveBalloons(0);
                var alreadyCovered = GetCoveredCells();

		        var previousScores = new ScoreAndInstruction[_problem.NbLines,_problem.NbCols,8];
		        var currentScores = new ScoreAndInstruction[_problem.NbLines,_problem.NbCols,8];

		        //init t0
		        short firstR, firstC;
		        ApplyWind(_problem.DepartBallons.R, _problem.DepartBallons.C, 0, out firstR, out firstC);
                tree[0].Move = 1; //the first instruction of all paths : lift off
                tree[0].ParentIdx = -1;
                _freeIdx = 1;

		        for (int t = 1 /*turn 0 is hardcoded*/; t < _problem.NbTours; t++)
		        {
                    //reset score cache
                    _scoresCache = new int[75, 300];

		            //a balloon can lift off any turn from the starting cell
		            var scoreOnFirstCell = GetScoreAt(firstR, firstC, alreadyCovered);
                    if (previousScores[firstR, firstC, 0].Score < scoreOnFirstCell)
                        previousScores[firstR, firstC, 0] = new ScoreAndInstruction
                        {
                            Score = scoreOnFirstCell,
                            InstructionIdx = 0,
                        };

                    MoveBalloons(t);
                    alreadyCovered = GetCoveredCells();

		            for (int r = 0; r < _problem.NbLines; r++)
		            {
		                for (int c = 0; c < _problem.NbCols; c++)
		                {
		                    for (int a = 0; a < 8; a++)
		                    {
		                        var prevScore = previousScores[r, c, a];
		                        if (prevScore.Score <= 0)
		                            continue;

		                        //try all moves from current position
		                        var lower = a == 0 ? 0 : -1;
		                        var upper = a == 7 ? 0 : 1;
		                        for (int da = lower; da <= upper; da++)
		                        {
		                            short newR, newC;
		                            if (ApplyWind(r, c, a + da, out newR, out newC))
		                            {
		                                int oldScore = currentScores[newR, newC, a + da].Score;
		                                int newScore = prevScore.Score + GetScoreAt(newR, newC, alreadyCovered);
		                                if (newScore > oldScore)
		                                {
		                                    currentScores[newR, newC, a + da].Score = newScore;
		                                    tree[_freeIdx].Move = da;
		                                    tree[_freeIdx].ParentIdx = prevScore.InstructionIdx;
		                                    currentScores[newR, newC, a + da].InstructionIdx = _freeIdx;
		                                    _freeIdx++;
		                                }
		                            }
		                        }
		                    }
		                }
		            }
		            //swap matrixes
		            var tmp = previousScores;
		            previousScores = currentScores;
		            currentScores = tmp;
		        }

		        var stepIdx = GetBestScore(previousScores);
		        FillSolutionWith(tree[stepIdx], b);
                Console.WriteLine("(time is " + sw.Elapsed + ")");
            }
		    Console.WriteLine("total time : " + sw.Elapsed + "ms");
		    return _solution;
		}

        private int[,] _scoresCache;

	    private int GetScoreAt(short r, short c, byte[,] alreadyCovered)
	    {
	        if (_scoresCache[r, c] != 0)
	            return _scoresCache[r, c];

	        var hovered = _problem.GetListOfTargetReachedFrom(r, c);
	        int score = 0;
	        for (int i = 0; i < hovered.Count; i++)
	        {
	            var loc = hovered[i];
	            if (alreadyCovered[loc.R, loc.C] == 0)
	                score++;
	        }
	        _scoresCache[r, c] = score;
	        return score;
	    }

	    private void MoveBalloons(int turn)
        {
            for (int b = 0; b < 53; b++)
            {
                var pos = _placedBalloonsPosition[b];
                _placedBalloonsAltitude[b] += _solution.Moves[turn, b];
                short newR, newC;
                ApplyWind(pos.R, pos.C, _placedBalloonsAltitude[b] - 1, out newR, out newC);
                _placedBalloonsPosition[b] = new Vec2(newR, newC);
            }
	    }

	    private byte[,] GetCoveredCells()
	    {
	        var covered = new byte[75,300];
	        for (int b = 0; b < 53; b++)
	        {
	            if (_placedBalloonsAltitude[b] == 0)
	                continue; //have not left base yet

	            foreach (var loc in _problem.GetListOfTargetReachedFrom(_placedBalloonsPosition[b]))
	            {
	                covered[loc.R, loc.C] = 1;
	            }
	        }
	        return covered;
	    }

	    private void FillSolutionWith(InstructionNode step, int balloonIdx)
	    {
	        int turn = 399;
	        while (step.ParentIdx != -1)
	        {
                _solution.Moves[turn, balloonIdx] = step.Move;
	            step = tree[step.ParentIdx];
	            turn--;
	        }
            _solution.Moves[turn, balloonIdx] = step.Move; //last (=first) move
	    }

	    private static int GetBestScore(ScoreAndInstruction[,,] scores)
	    {
	        var max = new ScoreAndInstruction();
	        for (int r = 0; r < 75; r++)
	        {
	            for (int c = 0; c < 300; c++)
	            {
	                for (int a = 0; a < 8; a++)
	                {
	                    if (scores[r, c, a].Score > max.Score)
	                    {
	                        max = scores[r, c, a];
	                    }
	                }
	            }
	        }
	        Console.WriteLine("score = " + max.Score);

	        return max.InstructionIdx;
	    }

	    /// <returns>false if the new location is outside of the map</returns>
        private bool ApplyWind(int r, int c, int a, out short newR, out short newC)
        {
            var wind = _problem.GetCaze(r, c).Winds[a+1];
            newC = (short) ((c + wind.C)%300);
            if (newC < 0) newC += 300;
            newR = (short) (r + wind.R);
            return newR >= 0 && newR < 75;
        }
    }
}
