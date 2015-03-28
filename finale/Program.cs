using System;

namespace finale
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var problem =  Parser.Parse();

			var solver = new Solver (problem);
			var solution = solver.Solve ();

			Dumper.Dump (solution);
		}
	}
}
