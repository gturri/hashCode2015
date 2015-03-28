using System;
using System.Collections.Generic;

namespace finale
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            var problem = Parser.Parse("../../final_round.in");

            // Display map
		    var mapPrinter = new MapPrinter();
            mapPrinter.PrintMap(problem);



			var solver = new Solver (problem);
			var solution = solver.Solve ();

			Dumper.Dump (solution);
		}
	}
}
