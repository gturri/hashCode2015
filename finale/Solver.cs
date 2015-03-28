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
		   

            while (solution.currentTurn < _problem.NbTours - 1)
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
                var deltaAlt = newAltAndLocation.Key - balloon.Altitude;
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
            // !!!! do not cover the same target twice if possible
	        var nextPossiblePositions = Problem.FindNextPossiblePostions(_problem, balloon.Location, balloon.Altitude);
            
            // out of those 3, excluse those that that have negative loc (out)
            for (int i = 0; i < 3; i++)
                if (nextPossiblePositions[i].Line == -1)
                    nextPossiblePositions[i] = null;


            // and pick the one with the most cover
	        var altitudeChangeToScoreAndLoc = new Dictionary<int, Tuple<int, Localisation>>();
            if (nextPossiblePositions[0] != null)
                altitudeChangeToScoreAndLoc.Add(-1, new Tuple<int, Localisation>(
                    _problem.GetNbTargetsReachedFrom(nextPossiblePositions[0].Line, nextPossiblePositions[0].Col),
                    new Localisation(nextPossiblePositions[0].Line, nextPossiblePositions[0].Col)));
            if (nextPossiblePositions[1] != null)
                altitudeChangeToScoreAndLoc.Add(0, new Tuple<int, Localisation>(
                    _problem.GetNbTargetsReachedFrom(nextPossiblePositions[1].Line, nextPossiblePositions[1].Col),
                    new Localisation(nextPossiblePositions[1].Line, nextPossiblePositions[1].Col)));            
            
            if (nextPossiblePositions[2] != null)
                altitudeChangeToScoreAndLoc.Add(1, new Tuple<int, Localisation>(
                    _problem.GetNbTargetsReachedFrom(nextPossiblePositions[2].Line, nextPossiblePositions[2].Col),
                    new Localisation(nextPossiblePositions[2].Line, nextPossiblePositions[2].Col)));

	        int maxScore = -1;
	        var maxEntry = new KeyValuePair<int, Localisation>(0, new Localisation(-1, -1));
	        foreach (var kvp in altitudeChangeToScoreAndLoc)
	        {
	            if (kvp.Value .Item1>= maxScore) // /!\ introduction d'un biais vers les hautes altitudes grace à >=
	            {
	                maxScore = kvp.Value.Item1;
	                maxEntry = new KeyValuePair<int, Localisation>(kvp.Key, kvp.Value.Item2);
	            }
	        }

	        return maxEntry;
	    }
	}
}

