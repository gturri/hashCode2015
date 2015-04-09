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
        Balloons, //remove N balloons from an existing good solution and re-route them
        Turns, //remove N last turns from an existing good solution and re-route balloons
    }

	class MainClass
	{
	    [STAThread]
		public static void Main (string[] args)
		{
            var problem = Parser.Parse("../../final_round.in");

            string startingFile = null;
	        if (args.Length > 0)
	        {
	            if (args[0].StartsWith("-"))
	            {
	                if (args.Length > 1)
	                    startingFile = args[1];
	            }
	            else
	                startingFile = args[0];
	        }
	        var startingSolution = ReadStartingSolution(startingFile, problem);

	        var bestSolution = startingSolution.Clone();

            int initialScore = Scorer.Score(startingSolution);
	        var solver = new DynaSolver(problem, startingSolution);
	        while (true)
	        {
                //ApplyPerturbation(startingSolution, Helper.rand.NextDouble() < 0.5 ? Perturbator.Balloons : Perturbator.Turns);
                var stabilizedScore = solver.Solve(initialScore);
	            if (stabilizedScore > initialScore)
	            {
	                initialScore = stabilizedScore;
	                bestSolution = startingSolution.Clone();
	            }
	            else
	            {
	                startingSolution = (Solution) bestSolution;
	            }
	        }
		}

        private static Solution ReadStartingSolution(string solutionFile, Problem problem)
	    {
	        var startingSolution = new Solution(problem);//Helper.GetRandom(problem);
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
                case Perturbator.Balloons:
                    int nbBalloons = Helper.rand.Next(1, 5);
                    for (int t = 0; t < 400; t++)
                        for (int b = 1; b < nbBalloons; b++)
                            s.Moves[t, b] = 0;
                    Console.WriteLine("removed {0} balloons", nbBalloons+1);
                    break;
                case Perturbator.Turns:
                    int nbTurns = Helper.rand.Next(1, 6);
                    for (int t = 399; t >= nbTurns; t--)
                        for (int b = 0; b < 53; b++)
                            s.Moves[t, b] = s.Moves[t-nbTurns, b];
                    for (int b = 0; b < 53; b++)
                    {
                        for (int t = 0; t < nbTurns; t++)
                        {
                            s.Moves[t, b] = 0;
                        }
                    }
                    Console.WriteLine("removed {0} turns", nbTurns);
                    break;
            }
        }
	}
}
