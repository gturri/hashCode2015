using System;

namespace finale
{
	public struct Vector
	{
        public short DeltaRow { get; private set; }
        public short DeltaCol { get; private set; }

        public Vector(short deltaRow, short deltaCol)
            : this()
		{
			DeltaRow = deltaRow;
			DeltaCol = deltaCol;
		}

		private static Vector _emptyVector = new Vector(0, 0);

		public static Vector EmptyVector {get {return _emptyVector; }}
	}
}
