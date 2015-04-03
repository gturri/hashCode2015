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

        public byte[,] Targets = new byte[75,300];
        public Vec2[,,] Winds = new Vec2[75, 300, 9];

		public int NbAvailableBallons { get; private set; }

        public Problem(short nbLines, short nbCols, short nbAltitudes, short rayonCouverture, short nbTours, Vec2 departBallons, List<List<Caze>> cazes, short nbAvailableBallons)
		{
			NbLines = nbLines;
			NbCols = nbCols;
			NbAltitudes = nbAltitudes;
			RayonCouverture = rayonCouverture;
			NbTours = nbTours;
			DepartBallons = departBallons;
			NbAvailableBallons = nbAvailableBallons;

            for (int i = 0; i < 75; i++)
            {
                for (int j = 0; j < 300; j++)
                {
                    for (int k = 1; k < 9; k++)
                        Winds[i, j, k] = cazes[i][j].Winds[k];
                    if (cazes[i][j].IsTarget)
                        Targets[i, j] = 1;
                }
            }
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
                if (Targets[loc.R, loc.C] > 0) list.Add(loc);
            }

            _cacheTargets[r,c] = list;
            return list;
        }
	}
}
