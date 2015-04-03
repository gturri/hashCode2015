using System;

namespace finale
{
	public class Caze
	{
		public Vec2[] Winds { get; set; }
		public bool IsTarget { get; set; }

		public Caze (int altitudeMax)
		{
			Winds = new Vec2[altitudeMax+1];
		}
	}
}
