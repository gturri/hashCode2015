using System;
using System.Collections.Generic;

namespace finale
{
	public class GtDummySolver
	{
		private Problem _problem;

		public GtDummySolver (Problem problem)
		{
			_problem = problem;
		}

		public Solution Solve(){
			Solution sol = new Solution(_problem);
			for (int tour = 0; tour < _problem.NbTours; tour++) {
				List<int> commands = new List<int> ();
				for (int b=0; b < _problem.NbAvailableBallons; b++) {
					commands.Add (0);
				}
				sol.Moves.Add (commands.ToArray ());
			}
		    return sol;
		}
	}
}

