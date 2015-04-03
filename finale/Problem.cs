using System.Collections.Generic;

namespace finale
{
	public class Problem
	{
        public short NbLines { get; private set; }
        public short NbCols { get; private set; }
        public short NbAltitudes { get; private set; }

        public short RayonCouverture { get; private set; }
        public short NbTours { get; private set; }

        public Vec2 DepartBallons { get; private set; }

        public Caze GetCaze(Vec2 loc)
        {
			return GetCaze (loc.R, loc.C);
		}

		public Caze GetCaze (int r, int c){
            return _cazes[r][c];
		}

		public int NbAvailableBallons { get; private set; }

		private List<List<Caze>> _cazes;

        public Problem(short nbLines, short nbCols, short nbAltitudes, short rayonCouverture, short nbTours, Vec2 departBallons, List<List<Caze>> cazes, short nbAvailableBallons)
		{
			NbLines = nbLines;
			NbCols = nbCols;
			NbAltitudes = nbAltitudes;

			RayonCouverture = rayonCouverture;
			NbTours = nbTours;

			DepartBallons = departBallons;
			_cazes = cazes;

			NbAvailableBallons = nbAvailableBallons;
		}

        private List<Vec2> GetCazesReachedFrom(short r, short c)
		{
            var reached = new List<Vec2>();
            for (short i = -7; i <= 7; i++)
			{
                for (short j = -7; j <= 7; j++)
				{
					if (i * i + j * j > 7 * 7)
						continue; //not in range

                    short cazeC = (short)((c + i) % 300);
					if (cazeC < 0)
						cazeC += 300;

                    short cazeR = (short)(r + j);

					if (cazeR < 0 || cazeR >= 75) //boundaries check
						continue;

                    reached.Add(new Vec2(cazeR, cazeC));
				}
			}
			return reached;
		}

        readonly List<Vec2>[,] _cacheTargets = new List<Vec2>[75, 300];

        public List<Vec2> GetListOfTargetReachedFrom(Vec2 l)
        {
            return GetListOfTargetReachedFrom(l.R, l.C);
        }

        public List<Vec2> GetListOfTargetReachedFrom(short r, short c)
        {
            List<Vec2> fromCache = _cacheTargets[r, c];
            if (fromCache != null)
            {
                return fromCache;
            }

            var list = new List<Vec2>();
            foreach (Vec2 loc in GetCazesReachedFrom(r, c))
            {
                if (GetCaze(loc).IsTarget) list.Add(loc);
            }

            _cacheTargets[r,c] = list;
            return list;
        }
	}
}
