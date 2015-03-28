using System;
using System.Collections.Generic;

namespace finale
{
	public class Solver
	{
		public Solver (object problem)
		{
			throw new NotImplementedException ();
		}

		public Solution Solve (Problem problem)
		{
		    var solution = new Solution(problem);
		    var balloons = new Dictionary<int, Balloon>();
		    InitializeBalloons(balloons);

            while (solution.currentTurn < 400)
            {
                balloons = PlayTurn(balloons, solution);
            }
        }



        // play 1 turn by moving all baloons
        public Dictionary<int, Balloon> PlayTurn(Dictionary<int, Balloon> balloons, Solution solution)
        {
            // create new result structure
            solution.currentTurn++;
            solution.Moves[solution.currentTurn] = new int[53];
            var nextBalloons = new Dictionary<int, Balloon>();

            foreach (var balloon in balloons)
            {
                // find out next position for balloon
                KeyValuePair<int, Localisation> newAltAndLocation = GetNextAltAndLocation(balloon.Value);
                var deltaAlt = balloon.Value.Altitude - newAltAndLocation.Key;

                // update coordinates
                balloon.Value.Altitude = newAltAndLocation.Key;
                balloon.Value.Location = newAltAndLocation.Value;

                // dump move in soluton at current turn
                solution.RegisterBaloonMove(balloon.Key, deltaAlt);
                // solution.RegisterBalloon(balloon);


            }


            
        }



	    private KeyValuePair<int, Localisation> GetNextAltAndLocation(Balloon balloon)
	    {
            // use FindNextPossiblePostions(Problem problem, Localisation currentLoc, int currentAltitude)
            // to find a new position that is not out of the map
	        throw new NotImplementedException();
	    }
	}
}

