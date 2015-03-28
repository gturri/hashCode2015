using System;
using System.Collections.Generic;

namespace finale
{
	public class Solver
	{
	    public Problem _problem;

		public Solver (Problem problem)
		{
		    _problem = problem;
		}

		public Solution Solve (Problem problem)
		{
		    var solution = new Solution(problem);
            var balloons = InitializeBalloons();
		   

            while (solution.currentTurn < 400)
            {
                balloons = PlayTurn(balloons, solution);
            }

		    return solution;
		}

        private Balloon[] InitializeBalloons()
	    {
	        var balloons = new Balloon[53];
            for (int i = 0; i < 53; i++)
            {
                balloons[i] =  new Balloon(0, _problem.DepartBallons);
            }
	        return balloons;
	    }


	    // play 1 turn by moving all baloons
        // modify variables inplace :(
        public Balloon[] PlayTurn(Balloon[] balloons, Solution solution)
        {
            // create new result structure
            solution.currentTurn++;
            solution.Moves.Add(new int[53]);

            for (int b = 0; b < balloons.Length; b++)
			{
				var balloon = balloons [b];
				// find out next position for balloon
				KeyValuePair<int, Localisation> newAltAndLocation = GetNextAltAndLocation (balloon);
				var deltaAlt = balloon.Altitude - newAltAndLocation.Key;
				// update coordinates
				balloon.Altitude = newAltAndLocation.Key;
				balloon.Location = newAltAndLocation.Value;
				// dump move in soluton at current turn
				solution.RegisterBaloonMove (b, deltaAlt);
			}

            return balloons;
        }



	    private KeyValuePair<int, Localisation> GetNextAltAndLocation(Balloon balloon)
	    {
            // use FindNextPossiblePostions(Problem problem, Localisation currentLoc, int currentAltitude)
            // to find a new position that is not out of the map
	        var nextPossiblePositions = Problem.FindNextPossiblePostions(_problem, balloon.Location, balloon.Altitude);
            
            // out of those 3, excluse those that that have negative loc (out)

            // and pick the one with the most cover

            // if there is a choice, try to go to altitude 4


	        throw new NotImplementedException();
	    }
	}
}

