using System;
using System.Collections.Generic;
using System.Linq;

namespace finale
{
	public class Problem
	{
		public int NbLines { get; private set;}
		public int NbCols { get; private set;}
		public int NbAltitudes { get; private set;}

		public int RayonCouverture { get; private set;}
		public int NbTours { get; private set;}

		public Localisation DepartBallons { get; private set;}

		public Caze GetCaze(Localisation loc){
			return GetCaze (loc.Line, loc.Col);
		}

		public Caze GetCaze (int r, int c){
            return _cazes[r][c];
		}

		public int NbAvailableBallons { get; private set; }

		private List<List<Caze>> _cazes;

		private List<Localisation> _targets;
		public List<Localisation> Targets { get { return _targets; } }

		public Problem (int nbLines, int nbCols, int nbAltitudes, int rayonCouverture, int nbTours, Localisation departBallons, List<List<Caze>> cazes, int nbAvailableBallons)
		{
			NbLines = nbLines;
			NbCols = nbCols;
			NbAltitudes = nbAltitudes;

			RayonCouverture = rayonCouverture;
			NbTours = nbTours;

			DepartBallons = departBallons;
			_cazes = cazes;

			NbAvailableBallons = nbAvailableBallons;

			BuildTargets ();

			CheckTraps ();
		}

		void CheckTraps ()
		{
			bool cazeMarked = false;
			int traps = 0;

			do
			{
				cazeMarked = false;
				for (int r = 0; r < 75; r++)
				{
					for (int c = 0; c < 300; c++)
					{
						var caze = GetCaze (r, c);
						for (int a = 0; a < 9; a++)
						{
							if(caze.IsTrap [a])
								continue; //already seen

							bool canEscape = false;
							for (int d = -1; d < 2; d++)
							{
								if (a + d < 1 || a + d >= 9)
									continue;

								var wind = caze.Winds [a + d];
								if (r + wind.DeltaRow < 0 || r + wind.DeltaRow >= 75)
									continue;

								var newR = r + wind.DeltaRow;
								var newC = (c + wind.DeltaCol) % 300;
								if(newC < 0) newC += 300;
								if(GetCaze(newR, newC).IsTrap[a+d])
									continue;

								canEscape = true;
								break;
							}
							if (!canEscape)
							{
								caze.IsTrap [a] = true;
								cazeMarked = true;
								traps++;
							}
						}
					}
				}
			} while(cazeMarked);
		}

		private void BuildTargets(){
			_targets = new List<Localisation> ();
			for (int r=0; r < NbLines; r++) {
				for (int c=0; c < NbCols; c++) {
					if (GetCaze (r, c).IsTarget) {
						_targets.Add (new Localisation (r, c));
					}
				}
			}
		}

		public List<Localisation> GetCazesReachedFrom(Localisation loc){
			return GetCazesReachedFrom (loc.Line, loc.Col);
		}

        Dictionary<Localisation, List<Localisation>> _cache = new Dictionary<Localisation, List<Localisation>>();
        Dictionary<Localisation, List<Localisation>> _cacheTargets = new Dictionary<Localisation, List<Localisation>>();

		public List<Localisation> GetCazesReachedFrom(int r, int c)
		{
			List<Localisation> fromCache;
			Localisation cacheKey = new Localisation (r, c);
			if (_cache.TryGetValue (cacheKey, out fromCache)) {
				return fromCache;
			}

			var reached = new List<Localisation> ();
			for (int i = -6; i < 7; i++)
			{
				for (int j = -6; j < 7; j++)
				{
					if (i * i + j * j > 7 * 7)
						continue; //not in range

					int cazeC = (c + i)%300;
					if (cazeC < 0)
						cazeC += 300;

					int cazeR = r + j;

					if (cazeR < 0 || cazeR >= 75) //boundaries check
						continue;

					reached.Add (new Localisation(cazeR, cazeC));
				}
			}
			_cache.Add (cacheKey, reached);
			return reached;
		}

		public int GetNbTargetsReachedFrom(int r, int c)
		{
		    int count = 0;
		    foreach (Localisation loc in GetCazesReachedFrom(r, c))
		    {
		        if (GetCaze(loc).IsTarget) count++;
		    }
		    return count;
		}

        public List<Localisation> GetListOfTargetReachedFrom(Localisation l)
        {
            return GetListOfTargetReachedFrom(l.Line, l.Col);
        }

	    public List<Localisation> GetListOfTargetReachedFrom(int r, int c)
        {
            List<Localisation> fromCache;
            Localisation cacheKey = new Localisation(r, c);
            if (_cacheTargets.TryGetValue(cacheKey, out fromCache))
            {
                return fromCache;
            }

            List<Localisation> list = new List<Localisation>();
            foreach (Localisation loc in GetCazesReachedFrom(r, c))
            {
                if (GetCaze(loc).IsTarget) list.Add(loc);
            }

            _cacheTargets.Add(cacheKey, list);
            return list;
        }

	    public int GetNbTargetsReachedFromWhileIgnoringList(int r, int c, List<Localisation>  targetsAlreadyCovered)
        {
            int count = 0;
            foreach (Localisation loc in GetCazesReachedFrom(r, c))
            {
                if (GetCaze(loc).IsTarget && !targetsAlreadyCovered.Contains(loc)) count++;
            }
            return count;
        }


	    // Give all 3 possible postions for a baloon at next turn in that order :
        // if altitude -1, if stable, if altitude +1
        public static List<Localisation> FindNextPossiblePostions(Problem problem, Localisation currentLoc, int currentAltitude)
        {
            // check winds at current
            var possiblePositions = new List<Localisation>();

            // -------- Altitude -1 -----------
            if (!IsAltitudeValid(problem, currentAltitude - 1))
                possiblePositions.Add(new Localisation(-1, -1));
            else
            {
                var nextLine = currentLoc.Line + problem.GetCaze(currentLoc).Winds[currentAltitude - 1].DeltaRow;
                if (nextLine > problem.NbLines || nextLine < 0)
                    possiblePositions.Add(new Localisation(-1, -1));
                else
                {
                    int nextCaseAltmin1Col = (currentLoc.Col + problem.GetCaze(currentLoc).Winds[currentAltitude - 1].DeltaCol) % problem.NbCols;
                    possiblePositions.Add(new Localisation(nextLine, nextCaseAltmin1Col));
                }
            }

            // ---------- Altitude 0 -----------
            if (!IsAltitudeValid(problem, currentAltitude))
            {
                possiblePositions.Add(new Localisation(-1, -1));
            }
            else
            {
                var nextLine = currentLoc.Line + problem.GetCaze(currentLoc).Winds[currentAltitude].DeltaRow;
                if (nextLine > problem.NbLines || nextLine < 0)
                    possiblePositions.Add(new Localisation(-1, -1));
                else
                {
                    int nextCaseAlt0Col = (currentLoc.Col + problem.GetCaze(currentLoc).Winds[currentAltitude].DeltaCol) % problem.NbCols;
                    possiblePositions.Add(new Localisation(nextLine, nextCaseAlt0Col));
                }

            }

            // ---------- Altitude +1 ------------
            if (!IsAltitudeValid(problem, currentAltitude + 1))
            {
                possiblePositions.Add(new Localisation(-1, -1));
            }
            else
            {
                var nextLine = currentLoc.Line + problem.GetCaze(currentLoc).Winds[currentAltitude + 1].DeltaRow;
                if (nextLine > problem.NbLines || nextLine < 0)
                    possiblePositions.Add(new Localisation(-1, -1));
                else
                {
                    int nextCaseAltmax1Col = (currentLoc.Col + problem.GetCaze(currentLoc).Winds[currentAltitude + 1].DeltaCol) % problem.NbCols;
                    possiblePositions.Add(new Localisation(nextLine, nextCaseAltmax1Col));
                }
            }

            return possiblePositions;
        }

        public static bool IsAltitudeValid(Problem problem,int altitude)
        {
            if (altitude > problem.NbAltitudes || altitude <= 0)
                return false;
            return true;
        }

	}
}
