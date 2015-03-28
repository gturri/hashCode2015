using System;

namespace finale
{
	public class Caze
	{
		public Vector[] Winds { get; set; }
		public bool IsTarget { get; set; }

		public Localisation Localisation {get; set;}

		public Caze (int altitude)
		{
			Winds = new Vector[altitude+1];
			//TODO: add empty vector at altitude 0
		}
	}
}

