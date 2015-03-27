using System;

namespace HashCodePizza
{
	public class Parser
	{

		public static bool[,] Read(string file){
			string[] text = File.ReadAllLines(file);
			string[] firstLine = text[0].Split(' ');

			int nbLines = int.Parse (firstLine [0]);
			int nbCols = int.Parse (firstLine [1]);

			bool[,] result = new bool[nbLines, nbCols];
			for (int i=1; i <= nbLines; i++) {
				string line = text [i];
				for (int c = 0; c < nbCols; c++) {
					char ch = char.Parse(line[c]);
					result [i, c] = (ch == 'H');
				}
			}
			return result;
		}
	}
}

