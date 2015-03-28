using System;

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

		public Localisation[] Targets { get; private set;}

		public Problem (int nbLines, int nbCols, int nbAltitudes, int rayonCouverture, int nbTours, Localisation departBallons, Localisation[] targets)
		{
			NbLines = nbLines;
			NbCols = nbCols;
			NbAltitudes = nbAltitudes;

			RayonCouverture = rayonCouverture;
			NbTours = nbTours;

			DepartBallons = departBallons;
			Targets = targets;
		}
	}
}

