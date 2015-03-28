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

		public Solution Solve ()
		{
			throw new NotImplementedException ();
		}



        // play 1 turn by moving all baloons
        public List<Balloon> PlayTurn(Dictionary<int, Balloon> balloons, Solution solution)
        {
            // create new result structure
            solution.currentTurn++;
            solution.Moves[solution.currentTurn] = new int[53];

            foreach (var balloon in balloons)
            {
                // find out next position for balloon
                KeyValuePair<int, Localisation> newAltAndLocation = GetNextAltAndLocation(balloon.Value);
                var deltaAlt = balloon.Value.Altitude - newAltAndLocation.Key;

                // update coordinates
                balloon.Value.Altitude = newAltAndLocation.Key;
                balloon.Value.Location = newAltAndLocation.Value;

                // dump move in soluton
                solution.RegisterBaloonMove(balloon.Key, deltaAlt);
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

