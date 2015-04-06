using System;
using System.Collections.Generic;
using System.IO;

namespace finale
{
	class MainClass
	{

        [STAThread]
		public static void Main (string[] args)
		{
            var problem = Parser.Parse("../../final_round.in");

            string startingFile = null;

            var startingSolution = ReadStartingSolution(startingFile, problem);

            var solver = new DynaSolver(problem, startingSolution);
			solver.Solve();
		}

        private static Solution ReadStartingSolution(string solutionFile, Problem problem)
	    {
	        var startingSolution = new Solution(problem);
	        if (solutionFile != null)
	        {
	            using (var reader = new StreamReader(solutionFile))
	            {
	                int turn = 0;
	                string line;
	                while ((line = reader.ReadLine()) != null)
	                {
	                    var moves = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
	                    for (int b = 0; b < moves.Length; b++)
	                    {
	                        startingSolution.Moves[turn, b] = Int32.Parse(moves[b]);
	                    }
	                    turn++;
	                }
	            }
	        }
	        return startingSolution;
	    }
	}
}
