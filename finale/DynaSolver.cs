#define DRAW

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace finale
{
	public class DynaSolver
	{
	    public Problem _problem;

		public DynaSolver (Problem problem)
		{
		    _problem = problem;
		}

		public Solution Solve (Problem problem)
		{
		    var solution = new Solution(problem);

            var prevScores = new int[problem.NbLines, problem.NbCols, 8];
            //init t0
		    prevScores[problem.DepartBallons.Line, problem.DepartBallons.Col, 0] = problem.GetNbTargetsReachedFrom(problem.DepartBallons.Line, problem.DepartBallons.Col);

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
                var scores = new int[problem.NbLines, problem.NbCols, 8];
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
		                    maxScore = Math.Max(maxScore, prevScores[r, c, a]);
#endif
		                    var prevScore = prevScores[r, c, a];
		                    if (prevScore <= 0)
		                        continue;

                            //try all moves from current position
		                    var lower = a == 0 ? 0 : -1;
		                    var upper = a == 7 ? 0 : 1;
		                    for (int da = lower; da <= upper; da++)
		                    {
		                        short newR, newC;
		                        if (ApplyWind(r, c, a + da, out newR, out newC))
		                            scores[newR, newC, a + da] = Math.Max(
		                                scores[newR, newC, a + da],
		                                prevScore + problem.GetNbTargetsReachedFrom(newR, newC));
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
		        prevScores = scores;
		    }

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
