using System;
using System.Collections.Generic;

namespace finale
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            var problem = Parser.Parse("../../final_round.in");


			var solver = new Solver (problem);
			var solution = solver.Solve(problem);

			Dumper.Dump (solution);
		}
	}
}
