using System;

namespace finale
{
	public struct Vector
	{
		public int DeltaRow{ get; private set;}
		public int DeltaCol{ get; private set;}

		public Vector (int deltaRow, int deltaCol) : this()
		{
			DeltaRow = deltaRow;
			DeltaCol = deltaCol;
		}

		private static Vector _emptyVector = new Vector(0, 0);

		public static Vector EmptyVector {get {return _emptyVector; }}
	}
}
