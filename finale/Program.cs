using System;
using System.Collections.Generic;
using System.IO;

namespace finale
{
	class MainClass
	{
		public static Random rand = new Random ();

        [STAThread]
		public static void Main (string[] args)
		{
            var problem = Parser.Parse("../../final_round.in");

            var startingSolution = new Solution(problem);
            if (args.Length > 0)
            {
                var startingSolutionFile = args[0];
                using (var reader = new StreamReader(startingSolutionFile))
                {
                    int turn = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var moves = line.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                        for (int b = 0; b < moves.Length; b++)
                        {
                            startingSolution.Moves[turn, b] = Int32.Parse(moves[b]);
                        }
                        turn++;
                    }
                }
            }

			var solver = new DynaSolver(problem, startingSolution);
			solver.Solve();
		}
	}
}
