using System;

namespace finale
{
	public class Caze
	{
		public Vector[] Winds { get; set; }
		public bool IsTarget { get; set; }
		/// <summary>
		/// true if there is no way out of this caze at a specific altitude
		/// </summary>
		public bool[] IsTrap { get; set;}

		public Caze (int altitudeMax)
		{
			Winds = new Vector[altitudeMax+1];
			Winds [0] = Vector.EmptyVector;
			IsTrap = new bool[9];
		}
	}
}
