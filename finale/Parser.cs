using System;
using System.IO;

namespace finale
{
	public static class Parser
	{
		public static Problem Parse (string file)
		{
			string[] text = File.ReadAllLines(file);

			//1st line
			string[] firstLine = text[0].Split(' ');
			int nbLines = int.Parse (firstLine [0]);
			int nbCols = int.Parse (firstLine [1]);
			int nbAltitudes = int.Parse(firstLine[2]);

			//2nd line
			string[] secondLine = text [1].Split (' ');
			int nbCibles = int.Parse (secondLine [0]);
			int rayonCouverture = int.Parse (secondLine [1]);
			int nbAvailableBallons = int.Parse (secondLine [2]);
			int nbToursSimu = int.Parse (secondLine [3]);
		}
	}
}
