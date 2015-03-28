using System;
using System.Collections.Generic;

namespace finale
{
	public static class Scorer
	{
		public static int Score (Solution solution)
		{
			int result = 0;

			List<Balloon> ballons = BuildInitialBalloonList (solution);
			for (int tour=0; tour < solution.Moves.Count; tour++) {
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
			int[] deltaHs = solution.Moves [tour];
			System.Diagnostics.Debug.Assert (deltaHs.Length == ballons.Count);

			for (int i=0; i < deltaHs.Length; i++) {
				int prevAltitude = ballons [i].Altitude;

				if (deltaHs [i] > 1 || deltaHs [i] < -1) {
					throw new Exception ("deltaH out of range");
				}

				ballons [i].Altitude += deltaHs [i];
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
				if (ballon.Altitude == 0) {
					continue;
				}
				Vector move = solution.problem.GetCaze (ballon.Location).Winds [ballon.Altitude];
				int newLine = ballon.Location.Line + move.DeltaRow;
				int newCol = (ballon.Location.Col + move.DeltaCol) % solution.problem.NbCols;
				ballon.Location = new Localisation (newLine, newCol);
			}
		}

		private static void KillBallons(Solution solution, List<Balloon> ballons){
			foreach (var b in ballons) {
				if (b.Location.Line < 0 || b.Location.Line >= solution.problem.NbLines) {
					b.IsDead = true;
				}
			}
		}

		private static int ComputeInstantScore(Solution solution, List<Balloon> ballons){
			ISet<Localisation> covered = new HashSet<Localisation> ();

			foreach (var ballon in ballons) {
				if (!ballon.IsDead && ballon.Altitude > 0) {
					foreach (var loc in solution.problem.GetCazesReachedFrom(ballon.Location)) {
						covered.Add (loc);
					}
				}
			}

			int result = 0;
			foreach (var target in solution.problem.Targets) {
				if (covered.Contains (target)) {
					result++;
				}
			}

			return result;
		}
	}
}

