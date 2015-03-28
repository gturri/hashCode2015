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

			int max = -1;
			bool infinite = false;
			do
			{
				var solver = new Solver (problem);
				var solution = solver.Solve (problem);

				var score = Scorer.Score (solution);
				if (score > max)
				{
					max = score;
					Dumper.Dump (solution, score + ".txt");
					Console.WriteLine (score);
				}
			} while(infinite);
		}
	}
}
