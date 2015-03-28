using System;
using System.Collections.Generic;

namespace finale
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            var problem = Parser.Parse("../../final_round.in");


			var solver = new GtDummySolver (problem);
			var solution = solver.Solve();

			Console.WriteLine ("score: " + Scorer.Score (solution));

			Dumper.Dump (solution);
		}
	}
}
