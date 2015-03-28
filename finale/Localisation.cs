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

		public override int GetHashCode ()
		{
			return Line + Col * 75;
		}

		public override bool Equals (object obj)
		{
			var other = obj as Localisation;
			if (other == null) {
				return false;
			}

			return Line == other.Line && Col == other.Col;
		}

		public override string ToString ()
		{
			return "(" + Line + ", " + Col + ")";
		}
	}
}

