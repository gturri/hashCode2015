//#define DRAW

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

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
	    public Problem _problem;

		public DynaSolver (Problem problem)
		{
		    _problem = problem;
		}

		public Solution Solve (Problem problem)
		{
		    var sw = Stopwatch.StartNew();

		    var solution = new Solution(problem);

            var previousScores = new ScoreAndInstruction[problem.NbLines, problem.NbCols, 8];
            var currentScores = new ScoreAndInstruction[problem.NbLines, problem.NbCols, 8];
            //init t0
		    previousScores[problem.DepartBallons.Line, problem.DepartBallons.Col, 0] = new ScoreAndInstruction
		        {
		            Score = problem.GetNbTargetsReachedFrom(problem.DepartBallons.Line, problem.DepartBallons.Col),
		            Instruction = new InsctructionNode {Move = 1, Parent = null}, //root, we'll manage balloons lifting up later than turn 0 later
		        };

#if DRAW
            var turnImg = new Bitmap(300, 75);
		    for (int i = 0; i < 300; i++)
		    {
		        for (int j = 0; j < 75; j++)
		        {
		            turnImg.SetPixel(i, j, Color.Black);
		        }
		    }
#endif
		    for (int t = 1 /*turn 0 is hardcoded above*/; t < problem.NbTours; t++)
            {
                Console.WriteLine(t);
                for (int r = 0; r < problem.NbLines; r++)
		        {
		            for (int c = 0; c < problem.NbCols; c++)
                    {
#if DRAW
                        var maxScore = 0;
#endif
		                for (int a = 0; a < 8; a++)
		                {
#if DRAW
		                    maxScore = Math.Max(maxScore, previousScores[r, c, a]);
#endif
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
		                            currentScores[newR, newC, a + da].Score = Math.Max(
		                                currentScores[newR, newC, a + da].Score,
		                                prevScore.Score + problem.GetNbTargetsReachedFrom(newR, newC));
		                            currentScores[newR, newC, a + da].Instruction = new InsctructionNode
		                                {
		                                    Move = da,
		                                    Parent = prevScore.Instruction
		                                };
		                        }
		                    }
                        }
#if DRAW
                        if (maxScore > 0)
                        {
                            int cr, cg, cb;
                            HueToRgb((maxScore%5000)/5000.0, out cr, out cg, out cb);
                            turnImg.SetPixel(c, r, Color.FromArgb(cr, cg, cb));
                        }
#endif
		            }
                }
#if DRAW
                turnImg.Save(t + ".png", ImageFormat.Png);
#endif
                //swap matrixes
                var tmp = previousScores;
		        previousScores = currentScores;
                currentScores = tmp;
		    }

            ScoreAndInstruction max = new ScoreAndInstruction();
            for (int r = 0; r < problem.NbLines; r++)
		    {
		        for (int c = 0; c < problem.NbCols; c++)
		        {
		            for (int a = 0; a < 8; a++)
		            {
                        if (previousScores[r, c, a].Score > max.Score)
                        {
                            max = previousScores[r, c, a];
                        }
		            }
		        }
		    }
            Console.WriteLine("max = " + max.Score);

		    var step = max.Instruction;
		    int turn = 399;
		    while (step.Parent != null)
		    {
		        solution.Moves[turn, 0] = step.Move;
		        step = step.Parent;
		        turn--;
		    }
            solution.Moves[turn, 0] = step.Move; //last (=first) move
            
            Console.WriteLine("in " + sw.ElapsedMilliseconds + "ms");
		    return solution;
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

        void HueToRgb(double H, out int r, out int g, out int b)
        {
            H *= 360.0;
            double R, G, B;
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double qv = 1.0 * (1 - f);
                double tv = 1.0 * (1 - (1 - f));
                switch (i)
                {

                        // Red is the dominant color

                    case 0:
                        R = 1.0;
                        G = tv;
                        B = 0;
                        break;

                        // Green is the dominant color

                    case 1:
                        R = qv;
                        G = 1.0;
                        B = 0;
                        break;
                    case 2:
                        R = 0;
                        G = 1.0;
                        B = tv;
                        break;

                        // Blue is the dominant color

                    case 3:
                        R = 0;
                        G = qv;
                        B = 1.0;
                        break;
                    case 4:
                        R = tv;
                        G = 0;
                        B = 1.0;
                        break;

                        // Red is the dominant color

                    case 5:
                        R = 1.0;
                        G = 0;
                        B = qv;
                        break;

                        // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = 1.0;
                        G = tv;
                        B = 0;
                        break;
                    case -1:
                        R = 1.0;
                        G = 0;
                        B = qv;
                        break;
                        
                    default:
                        R = G = B = 1.0; // Just pretend its black/white
                        break;
                }
            }
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }
	}
}
