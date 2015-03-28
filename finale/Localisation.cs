using System;

namespace finale
{
	public class Localisation
	{
		public int Line { get; private set;}
		public int Col { get; private set;}

		public Localisation (int line, int col)
		{
			Line = line;
			Col = col;
		}
	}
}

