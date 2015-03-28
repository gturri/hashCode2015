using System;

namespace finale
{
	public class Caze
	{
		public Vector[] Winds { get; set; }
		public bool IsTarget { get; set; }

		public Caze (int altitudeMax)
		{
			Winds = new Vector[altitudeMax+1];
			Winds [0] = Vector.EmptyVector;
		}
	}
}

