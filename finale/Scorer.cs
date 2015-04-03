using System;
using System.Collections.Generic;

namespace finale
{
	public static class Scorer
	{
		public static int Score (Solution solution, int nbTours = 400)
		{
			int result = 0;

			List<Balloon> ballons = BuildInitialBalloonList (solution);
			for (int tour=0; tour < nbTours; tour++) {
				UpdatePositions (solution, ballons, tour);
				result += ComputeInstantScore (solution, ballons);
			}
			return result;
		}

		private static List<Balloon> BuildInitialBalloonList(Solution solution){
			List<Balloon> result = new List<Balloon> ();
			for (int b=0; b < solution.problem.NbAvailableBallons; b++) {
				result.Add (new Balloon (0, solution.problem.DepartBallons));
			}
			return result;
		}

		private static void UpdatePositions(Solution solution, List<Balloon> ballons, int tour){
			UpdateAltitudes (solution, ballons, tour);
			UpdateLatLon (solution, ballons);
			KillBallons (solution, ballons);
		}

		private static void UpdateAltitudes(Solution solution, List<Balloon> ballons, int tour){

			for (int i=0; i < 53; i++) {
				int prevAltitude = ballons [i].Altitude;

                if (solution.Moves[tour,i] > 1 || solution.Moves[tour,i] < -1)
                {
					throw new Exception ("deltaH out of range");
				}

                ballons[i].Altitude += solution.Moves[tour,i];
				if (ballons [i].Altitude > solution.problem.NbAltitudes) {
					throw new Exception ("Ballon is too high");
				}

				if (prevAltitude != 0 && ballons [i].Altitude == 0) {
					throw new Exception ("Ballon can't land");
				}
			}
		}

		private static void UpdateLatLon(Solution solution, List<Balloon> ballons){
			foreach (var ballon in ballons) {
				if (ballon.Altitude == 0 || ballon.IsDead) {
					continue;
				}
				Vec2 move = solution.problem.GetCaze (ballon.Location).Winds [ballon.Altitude];
				short newLine = (short)(ballon.Location.R + move.R);
                short newCol = (short) ((ballon.Location.C + move.C) % solution.problem.NbCols);
				if (newCol < 0)
					newCol += 300;
				ballon.Location = new Vec2 (newLine, newCol);
			}
		}

		private static void KillBallons(Solution solution, List<Balloon> ballons){
			foreach (var b in ballons) {
				if (b.Location.R < 0 || b.Location.R >= solution.problem.NbLines) {
					b.IsDead = true;
				}
			}
		}

		private static int ComputeInstantScore(Solution solution, List<Balloon> ballons){
            var covered = new bool[75,300];
            int result = 0;

		    for (int b = 0; b < ballons.Count; b++)
		    {
		        var ballon = ballons[b];
		        if (!ballon.IsDead && ballon.Altitude > 0)
		        {
                    var cazes = solution.problem.GetListOfTargetReachedFrom(ballon.Location);
		            for (int c = 0; c < cazes.Count; c++)
		            {
		                var loc = cazes[c];
		                if (!covered[loc.R, loc.C])
		                {
		                    covered[loc.R, loc.C] = true;
                            result++;
                        }
		            }
		        }
		    }
            
		    return result;
		}
	}
}

