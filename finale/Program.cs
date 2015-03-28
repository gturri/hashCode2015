using System;
using System.Collections.Generic;

namespace finale
{
	class MainClass
	{
		public static Random rand = new Random ();

		public static void Main (string[] args)
		{
            var problem = Parser.Parse("../../final_round.in");

			/*int max = 0;
			while (true)
			{
				var solver = new Solver (problem);
				var solution = solver.Solve (problem);

				var score = Scorer.Score (solution);
				if (score > max)
				{
					max = score;
					Dumper.Dump (solution);
					Console.WriteLine (score);
				}
			}*/

            var solver = new Solver(problem);
            var solution = solver.Solve(problem);
            Dumper.Dump(solution);
		}
	}
}
