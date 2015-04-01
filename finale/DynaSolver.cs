using System;
using System.Diagnostics;

namespace finale
{
    public class InsctructionNode
    {
        public int Move;
        public InsctructionNode Parent;
    }
    public struct ScoreAndInstruction
    {
        public int Score;
        public InsctructionNode Instruction;
    }

	public class DynaSolver
	{
	    private readonly Problem _problem;

		public DynaSolver (Problem problem)
		{
		    _problem = problem;
		}

		public Solution Solve (Problem problem)
		{
		    var sw = Stopwatch.StartNew();

            var previousScores = new ScoreAndInstruction[problem.NbLines, problem.NbCols, 8];
            var currentScores = new ScoreAndInstruction[problem.NbLines, problem.NbCols, 8];
            //init t0
		    short firstR, firstC;
		    ApplyWind(problem.DepartBallons.Line, problem.DepartBallons.Col, 0, out firstR, out firstC);
		    previousScores[firstR, firstC, 0] = new ScoreAndInstruction
		        {
                    Score = problem.GetNbTargetsReachedFrom(firstR, firstC),
		            Instruction = new InsctructionNode {Move = 1, Parent = null}, //root
		        };

		    for (int t = 1 /*turn 0 is hardcoded above*/; t < problem.NbTours; t++)
            {
                Console.WriteLine(t);
                for (int r = 0; r < problem.NbLines; r++)
		        {
		            for (int c = 0; c < problem.NbCols; c++)
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
		                            int newScore = prevScore.Score + problem.GetNbTargetsReachedFrom(newR, newC);
		                            if (newScore > oldScore)
		                            {
		                                currentScores[newR, newC, a + da].Score = newScore;
		                                currentScores[newR, newC, a + da].Instruction = new InsctructionNode
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
            var solution = BuildSolutionFrom(step, problem);

		    Console.WriteLine("in " + sw.ElapsedMilliseconds + "ms");
		    return solution;
		}

	    private static Solution BuildSolutionFrom(InsctructionNode step, Problem problem)
	    {
	        var solution = new Solution(problem);
	        int turn = 399;
	        while (step.Parent != null)
	        {
	            solution.Moves[turn, 0] = step.Move;
	            step = step.Parent;
	            turn--;
	        }
	        solution.Moves[turn, 0] = step.Move; //last (=first) move
	        //TODO : fill with zeros if turns remain
	        System.Diagnostics.Debug.Assert(turn == 0);
	        return solution;
	    }

	    private static InsctructionNode GetBestScore(ScoreAndInstruction[,,] scores)
	    {
	        var max = new ScoreAndInstruction();
	        for (int r = 0; r < 75; r++)
	        {
	            for (int c = 0; c < 400; c++)
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
	        Console.WriteLine("max = " + max.Score);

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
