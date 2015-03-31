using System;

namespace finale
{
	public struct Localisation
	{
		public short Line { get; private set;}
		public short Col { get; private set;}

		public Localisation (short line, short col):this()
		{
			Line = line;
			Col = col;
		}
	}
}

