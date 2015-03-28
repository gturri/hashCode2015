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

		private List<List<Caze>> _cazes;


		public Problem (int nbLines, int nbCols, int nbAltitudes, int rayonCouverture, int nbTours, Localisation departBallons, List<List<Caze>> cazes)
		{
			NbLines = nbLines;
			NbCols = nbCols;
			NbAltitudes = nbAltitudes;

			RayonCouverture = rayonCouverture;
			NbTours = nbTours;

			DepartBallons = departBallons;
			_cazes = cazes;
		}

		public List<Caze> GetCazesReachedFrom(int r, int c)
		{
			var reached = new List<Caze> ();
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

					if (cazeR < 0 || cazeR > 75) //boundaries check
						continue;

					reached.Add (GetCaze (cazeR, cazeC));
				}
			}
			return reached;
		}

		public int GetNbTargetsReachedFrom(int r, int c)
		{
			return GetCazesReachedFrom (r, c).Count(caze => caze.IsTarget);
		}
	}
}
