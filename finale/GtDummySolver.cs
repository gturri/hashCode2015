using System;
using System.Collections.Generic;

namespace finale
{
	public class GtDummySolver
	{
		private Problem _problem;
		private static Random _random = MainClass.rand;

		public GtDummySolver (Problem problem)
		{
			_problem = problem;
		}

		public Solution Solve(){
			return Solve (SolInit ());
		}

		public Solution Solve(Solution current){
			int bestScore = Scorer.Score (current);

			while (true) {
				int score;
				current = NextSol (current, out score);
				Console.WriteLine (score);
				if (score > bestScore) {
					bestScore = score;
					Dumper.Dump (current);
				}
			}
		}

		private Solution SolInit(){
			Solution sol = new Solution(_problem);
			for (int tour = 0; tour < _problem.NbTours; tour++) {
				List<int> commands = new List<int> ();
				for (int b=0; b < _problem.NbAvailableBallons; b++) {
					commands.Add (tour == 0 ? 1 : 0);
				}
/*				if (tour == 0) {
					commands [0] = 1;
				}*/
				sol.Moves.Add (commands.ToArray ());
			}
			return sol;
		}

		private Solution NextSol(Solution current, out int score){
			bool done = false;
			score = 0;
			while ( ! done ) {
				int b = _random.Next (0, _problem.NbAvailableBallons);
				int t = _random.Next(0, _problem.NbTours);
				int h = _random.Next(0, 3) - 2;

				Solution newSol = new Solution(_problem);

				List<int[]> newMoves = new List<int[]>();
				for ( int i=0 ; i < current.Moves.Count ; i++ ){
					newMoves.Add((int[]) current.Moves[i].Clone());
				}
				newMoves[t][b] = h;
				newSol.Moves = newMoves;

				try {
					score = Scorer.Score(newSol);
					return newSol;
				}catch(Exception){
					newSol = current;
				}
			}
			return null;

		}
	}
}

