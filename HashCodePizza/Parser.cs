using System;
using System.IO;

namespace HashCodePizza
{
	public class Parser
	{
		public static bool[,] Read(){
			return Read ("test_round.in");
		}

		public static int nbLines = 180;
		public static int nbCols = 60;

		public static bool[,] Read(string file){
			string[] text = File.ReadAllLines(file);
			string[] firstLine = text[0].Split(' ');

			int nbLines = int.Parse (firstLine [0]);
			int nbCols = int.Parse (firstLine [1]);

			bool[,] result = new bool[nbLines, nbCols];
			for (int l=1; l <= nbLines; l++) {
				string line = text [l];
				for (int c = 0; c < nbCols; c++) {
					char ch = line[c];
					result [l, c] = (ch == 'H');
				}
			}
			return result;
		}
	}
}

