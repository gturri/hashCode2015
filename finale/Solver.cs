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


            while (solution.currentTurn < _problem.NbTours)
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
            solution.Moves.Add(new int[53]);
            _targetsCoveredThisTurn = new List<Localisation>();

            for (int b = 0; b < balloons.Length; b++)
			{
				var balloon = balloons [b];
				// find out next position for balloon
				KeyValuePair<int, Localisation> newAltAndLocation = GetNextAltAndLocation (balloon);
                var deltaAlt = newAltAndLocation.Key ;
				// update coordinates
				balloon.Altitude = newAltAndLocation.Key + balloon.Altitude;
				balloon.Location = newAltAndLocation.Value;
				// dump move in soluton at current turn
				solution.RegisterBaloonMove (b, deltaAlt);
			}

           /* foreach (var s in solution.Moves[solution.currentTurn])
            {
                Console.Write(s + " ");
            }
            Console.WriteLine();*/


            solution.currentTurn++;
            return balloons;
        }

		private KeyValuePair<int, Localisation> GetNextAltAndLocationVandon(Balloon balloon)
		{
			if(balloon.Location.Col < 0 || balloon.Location.Line < 0 || balloon.Location.Col >= 300 || balloon.Location.Line >= 75)
				return new KeyValuePair<int, Localisation>(0, new Localisation(-1, -1));

			int move;
			int newLine;
			Vector wind;
		int tooManyLoops = 10;
			do
			{
				//move randomly up or down
				int lower = balloon.Altitude < 2 ? 0 : -1;
				var upper = balloon.Altitude == 8 ? 1 : 2;
				move = MainClass.rand.Next (lower, upper);

				//compute next location
				wind = _problem.GetCaze (balloon.Location).Winds [balloon.Altitude + move];
				newLine = balloon.Location.Line + wind.DeltaRow;

				if(tooManyLoops --< 0)
					break;
			} while(newLine < 0 || newLine >= 75); //don't kill baloons

			int newCol = (balloon.Location.Col + wind.DeltaCol) % _problem.NbCols;
			var newPos = new Localisation (newLine, newCol);

			return new KeyValuePair<int, Localisation> (move, newPos);
		}



	    private List<Localisation> _targetsCoveredThisTurn; 

	    private KeyValuePair<int, Localisation> GetNextAltAndLocation(Balloon balloon)
	    {
            if (balloon.Location.Col < 0 || balloon.Location.Line < 0 || balloon.Location.Col >= 300 || balloon.Location.Line >= 75)
                return new KeyValuePair<int, Localisation>(0, new Localisation(-1, -1));

	        var nextPossiblePositions = Problem.FindNextPossiblePostions(_problem, balloon.Location, balloon.Altitude);

            // out of those 3, excluse those that that have negative loc (out)
            for (int i = 0; i < 3; i++)
                if (nextPossiblePositions[i].Line == -1)
                    nextPossiblePositions[i] = null;


            // and pick the one with the most cover for uncovered targets
	        int scoremin1 = -1;
            int score0 = -1;
	        int scoremax1 = -1;
            if (nextPossiblePositions[0] != null)
                scoremin1 = _problem.GetNbTargetsReachedFromWhileIgnoringList(nextPossiblePositions[0].Line, nextPossiblePositions[0].Col, _targetsCoveredThisTurn);
	        if (nextPossiblePositions[1] != null)
                score0 = _problem.GetNbTargetsReachedFromWhileIgnoringList(nextPossiblePositions[1].Line, nextPossiblePositions[1].Col, _targetsCoveredThisTurn);
            if (nextPossiblePositions[2] != null)
                scoremax1 = _problem.GetNbTargetsReachedFromWhileIgnoringList(nextPossiblePositions[2].Line, nextPossiblePositions[2].Col, _targetsCoveredThisTurn);

            // if there is a positive score (cover an uncovered target !), use those scores
            // else use the score to cover the most targets
            var altitudeChangeToScoreAndLoc = new Dictionary<int, Tuple<int, Localisation>>();
            if (scoremin1 > 0 || score0 > 0 || scoremax1 > 0)
            {
                
                if (nextPossiblePositions[0] != null)
                    altitudeChangeToScoreAndLoc.Add(-1, new Tuple<int, Localisation>(
                        scoremin1,
                        new Localisation(nextPossiblePositions[0].Line, nextPossiblePositions[0].Col)));
                if (nextPossiblePositions[1] != null)
                    altitudeChangeToScoreAndLoc.Add(0, new Tuple<int, Localisation>(
                        score0,
                        new Localisation(nextPossiblePositions[1].Line, nextPossiblePositions[1].Col)));
                if (nextPossiblePositions[2] != null)
                    altitudeChangeToScoreAndLoc.Add(1, new Tuple<int, Localisation>(
                        scoremax1,
                        new Localisation(nextPossiblePositions[2].Line, nextPossiblePositions[2].Col)));


                // return the move with the best score
                int maxScore = -1;
                var maxEntry = new KeyValuePair<int, Localisation>(0, new Localisation(balloon.Location.Line, balloon.Location.Col));
                foreach (var kvp in altitudeChangeToScoreAndLoc)
                {
                    if (kvp.Value.Item1 >= maxScore) // /!\ introduction d'un biais vers les hautes altitudes grace à >=
                    {
                        maxScore = kvp.Value.Item1;
                        maxEntry = new KeyValuePair<int, Localisation>(kvp.Key, kvp.Value.Item2);
                    }
                }

                foreach (var localisation in _problem.GetListOfTargetReachedFrom(maxEntry.Value.Line, maxEntry.Value.Col))
                    _targetsCoveredThisTurn.Add(localisation);
                return maxEntry;

            }
            else // no new nodes covered : go to a random legal location
            {
                //move randomly up or down
               /* int lower = balloon.Altitude < 2 ? 0 : -1;
                var upper = balloon.Altitude == 8 ? 1 : 2;
                var targetAlt = MainClass.rand.Next(lower, upper);

                if (balloon.Altitude < targetAlt && nextPossiblePositions[2] != null)
                    return new KeyValuePair<int, Localisation>(1, new Localisation(nextPossiblePositions[2].Line, nextPossiblePositions[2].Col));
                if (balloon.Altitude == targetAlt && nextPossiblePositions[1] != null)
                    return new KeyValuePair<int, Localisation>(0, new Localisation(nextPossiblePositions[1].Line, nextPossiblePositions[1].Col));
                if (balloon.Altitude > targetAlt && nextPossiblePositions[0] != null)
                    return new KeyValuePair<int, Localisation>(-1, new Localisation(nextPossiblePositions[0].Line, nextPossiblePositions[0].Col));*/

                return new KeyValuePair<int, Localisation>(0, new Localisation(balloon.Location.Line, balloon.Location.Col));
            }



            // return the move with the best score
	       /* int maxScore = -1;
	        var maxEntry = new KeyValuePair<int, Localisation>(0, new Localisation(-1, -1));
	        foreach (var kvp in altitudeChangeToScoreAndLoc)
	        {
	            if (kvp.Value .Item1 >= maxScore) // /!\ introduction d'un biais vers les hautes altitudes grace à >=
	            {
	                maxScore = kvp.Value.Item1;
	                maxEntry = new KeyValuePair<int, Localisation>(kvp.Key, kvp.Value.Item2);
	            }
	        }*/

            // we select the maxEntry for this balloon next turn : 
            // mark all covered targets as out for the next turn
           /* foreach (var localisation in _problem.GetListOfTargetReachedFrom(maxEntry.Value.Line, maxEntry.Value.Col))
                _targetsCoveredThisTurn.Add(localisation);*/

	    }
	}
}
