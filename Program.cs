using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace HashCode
{
	class MainClass
	{
	    public static void Main (string[] args)
	    {
            // Read the file to get the problem parameters
	        var problemStatement = new Problem();

	        var solver = new Solver(problemStatement);
            var solution = solver.Solve();
/*
	        var scorer = new Scorer(problemStatement);
	        scorer.ComputeScore(solution);*/

            Dumper.Dump(solution);
	        Console.ReadKey();

           

	    }







	}
}
