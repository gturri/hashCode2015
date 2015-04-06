using System;
using System.Collections.Generic;
using System.IO;

namespace finale
{
    enum Perturbator
    {
        None, //just optimize from the starting file
        RandomStart, //start by taking a random solution, and improve on it
        Merge, //take balloons from 2 good solutions, and improve the frankensteined solution
        EraseN, //remove N balloons from an existing good solution and re-route them
    }

	class MainClass
	{
	    [STAThread]
		public static void Main (string[] args)
		{
            var problem = Parser.Parse("../../final_round.in");

            string startingFile = null;
	        var perturbator = Perturbator.None;
	        if (args.Length > 0)
	        {
	            if (args[0].StartsWith("-"))
	            {
	                perturbator = (Perturbator) Enum.Parse(typeof (Perturbator), args[0].Substring(1));
	                if (args.Length > 1)
	                    startingFile = args[1];
	            }
	            else
	                startingFile = args[0];
	        }
	        var startingSolution = ReadStartingSolution(startingFile, problem);
            ApplyPerturbation(startingSolution, perturbator);

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

        private static void ApplyPerturbation(Solution s, Perturbator p)
        {
            Helper.Shuffle(s);
            switch (p)
            {
                case Perturbator.RandomStart:
                    //TODO : revive VandonSolver
                    break;
                case Perturbator.Merge:
                    //TODO : be able to automatically chose 2 solutions that are not from the same branch
                    break;
                case Perturbator.EraseN:
                    for (int t = 0; t < 400; t++)
                    {
                        s.Moves[t, 1] = 0;
                    }
                    break;
            }
        }
	}
}
