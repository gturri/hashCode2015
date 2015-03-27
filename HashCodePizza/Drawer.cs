using System;

namespace HashCodePizza
{
	public class Drawer
	{
		public static void Print(bool[,] pizza){
			for (int l = 0; l < Parser.nbLines; l++) {
				for (int c=0; c < Parser.nbCols; c++) {
					Console.Write (pizza[l,c] ? 'H' : '-');
				}
				Console.WriteLine ();
			}
		}
	}
}

