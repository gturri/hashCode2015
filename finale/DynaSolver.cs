using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace finale
{
    public class InstructionNode
    {
        public int Move;
        public InstructionNode Parent;
    }
    public struct ScoreAndInstruction
    {
        public int Score;
        public InstructionNode Instruction;
    }

	public class DynaSolver
	{
	    private readonly Problem _problem;
	    private readonly Solution _solution;
        private readonly Localisation[] _placedBalloonsPosition = new Localisation[53];
        private readonly int[] _placedBalloonsAltitude = new int[53];

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
                MoveBalloons(b, 0);
                var alreadyCovered = GetCoveredCells();

		        var previousScores = new ScoreAndInstruction[_problem.NbLines,_problem.NbCols,8];
		        var currentScores = new ScoreAndInstruction[_problem.NbLines,_problem.NbCols,8];

		        //init t0
		        short firstR, firstC;
		        ApplyWind(_problem.DepartBallons.Line, _problem.DepartBallons.Col, 0, out firstR, out firstC);
		        var rootInstruction = new InstructionNode {Move = 1, Parent = null}; //the first instruction of all paths : lift off

		        for (int t = 1 /*turn 0 is hardcoded*/; t < _problem.NbTours; t++)
		        {
		            //a balloon can lift off any turn from the starting cell
		            var scoreOnFirstCell = GetScoreAt(firstR, firstC, alreadyCovered);
                    if (previousScores[firstR, firstC, 0].Score < scoreOnFirstCell)
                        previousScores[firstR, firstC, 0] = new ScoreAndInstruction
                        {
                            Score = scoreOnFirstCell,
                            Instruction = rootInstruction,
                        };

                    MoveBalloons(b, t);
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
		                                    currentScores[newR, newC, a + da].Instruction = new InstructionNode
		                                        {
		                                            Move = da,
		                                            Parent = prevScore.Instruction
		                                        };
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

		        var step = GetBestScore(previousScores);
		        FillSolutionWith(step, b);
                Console.WriteLine("(time is " + sw.Elapsed + ")");
            }
		    Console.WriteLine("total time : " + sw.Elapsed + "ms");
		    return _solution;
		}

	    private int GetScoreAt(short r, short c, bool[,] alreadyCovered)
	    {
	        var hovered = _problem.GetListOfTargetReachedFrom(r, c);
	        int score = 0;
	        foreach (var loc in hovered)
	        {
	            if (!alreadyCovered[loc.Line, loc.Col])
	                score++;
	        }
	        return score;
	    }

	    private void MoveBalloons(int nbBalloons, int turn)
        {
            for (int b = 0; b < nbBalloons; b++)
            {
                var pos = _placedBalloonsPosition[b];
                _placedBalloonsAltitude[b] += _solution.Moves[turn, b];
                short newR, newC;
                ApplyWind(pos.Line, pos.Col, _placedBalloonsAltitude[b] - 1, out newR, out newC);
                _placedBalloonsPosition[b] = new Localisation(newR, newC);
            }
	    }

	    private bool[,] GetCoveredCells()
	    {
	        var covered = new bool[75,300];
	        for (int b = 0; b < 53; b++)
	        {
	            if (_placedBalloonsAltitude[b] == 0)
	                continue; //have not left base yet

	            foreach (var loc in _problem.GetListOfTargetReachedFrom(_placedBalloonsPosition[b]))
	            {
	                covered[loc.Line, loc.Col] = true;
	            }
	        }
	        return covered;
	    }

	    private void FillSolutionWith(InstructionNode step, int balloonIdx)
	    {
	        int turn = 399;
	        while (step.Parent != null)
	        {
                _solution.Moves[turn, balloonIdx] = step.Move;
	            step = step.Parent;
	            turn--;
	        }
            _solution.Moves[turn, balloonIdx] = step.Move; //last (=first) move
	    }

	    private static InstructionNode GetBestScore(ScoreAndInstruction[,,] scores)
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

	        var step = max.Instruction;
	        return step;
	    }

	    /// <returns>false if the new location is outside of the map</returns>
        private bool ApplyWind(int r, int c, int a, out short newR, out short newC)
        {
            var wind = _problem.GetCaze(r, c).Winds[a+1];
            newC = (short) ((c + wind.DeltaCol)%300);
            if (newC < 0) newC += 300;
            newR = (short) (r + wind.DeltaRow);
            return newR >= 0 && newR < 75;
        }
    }
}
