using System;
using System.Collections.Generic;
using System.Threading;

namespace finale
{
	public class MainClass
	{
		public static Random rand = new Random ();
		public static Problem problem;

		public static void Main (string[] args)
		{
			problem = Parser.Parse ("../../final_round.in");
			List<Thread> ts = new List<Thread> ();
			for (int i=0; i < 4; i++) {
				var steps = (i+1) * 15;
				var t = new Thread (() => Work (steps));
				ts.Add(t);
				      t.Start();
			}

			foreach (var t in ts) {
				t.Join ();
			}

		}

		public static void Work(int vandonSteps){
			int max = -1;
			Solution bestSol = null;
			for (int i=0; i < vandonSteps; i++) {
				Console.WriteLine (i);
				var solver = new Solver (problem);
				var solution = solver.Solve (problem);

				var score = Scorer.Score (solution);
				if (score > max) {
					max = score;
					bestSol = solution;
					Dumper.Dump (solution, score + ".txt");
					Console.WriteLine (score);
				}
			}
			Console.WriteLine ("Switching to gt mode");
			new GtDummySolver (problem).Solve (bestSol);
		}
	}
}
