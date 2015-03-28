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

		public override int GetHashCode ()
		{
			return Line + Col * 75;
		}

		public override bool Equals (object obj)
        {
            if (!(obj is Localisation))
                return false;

            var other = (Localisation)obj;
			return Line == other.Line && Col == other.Col;
		}

		public override string ToString ()
		{
			return "(" + Line + ", " + Col + ")";
		}
	}
}

