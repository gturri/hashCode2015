using System;
using System.Collections.Generic;

namespace finale
{
	public class GtDummySolver
	{
		private Problem _problem;
		private Random _random = new Random();

		public GtDummySolver (Problem problem)
		{
			_problem = problem;
		}

		public Solution Solve(){
			Solution current = SolInit ();
			int bestScore = Scorer.Score (current);

			while (true) {
				current = NextSol (current);
				int score = Scorer.Score (current);
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

		private Solution NextSol(Solution current){
			bool done = false;
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
					Scorer.Score(newSol);
					return newSol;
				}catch(Exception){
					newSol = current;
				}
			}
			return null;

		}
	}
}

