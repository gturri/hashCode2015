using System;

namespace finale
{
	public struct Vector
	{
		public int startX{ get; private set;}
		public int startY{ get; private set;}
		public int dirX{ get; private set;}
		public int dirY{ get; private set;}

		public Vector (int startX, int startY, int dirX, int dirY)
		{
			this.dirY = dirY;
			this.dirX = dirX;
			this.startY = startY;
			this.startX = startX;
		}
	}
}
