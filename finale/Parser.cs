using System;
using System.IO;
using System.Collections.Generic;

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

			//3rd line
			string[] thirdLine = text [2].Split (' ');
			Localisation departBallon = new Localisation (line: int.Parse (thirdLine [0]), col: int.Parse (thirdLine [1]));

			//targets
			List<List<Caze>> cazes = new List<List<Caze>> ();
			for (int l = 0; l < nbLines; l++) {
				List<Caze> newLine = new List<Caze> ();
				for (int c = 0; c < nbCols; c++) {
					newLine.Add (new Caze (nbAltitudes));
				}
				cazes.Add (newLine);
			}

			int line = 3;
			for (; line < 3 + nbCibles; line ++) {
				String[] targetLine = text [line].Split (' ');
				int lineTarget = int.Parse(targetLine[0]);
				int colTarget = int.Parse(targetLine[1]);
				cazes [lineTarget] [colTarget].IsTarget = true;
			}

			//Winds
			for (int altitude=1; altitude <= nbAltitudes; altitude++) {
				for (int lineForGivenAltitude = 0; lineForGivenAltitude < nbLines; lineForGivenAltitude++) {
					line++;
					string[] altitudeLineStr = text [line].Split (' ');
				
					int charIdx = 0;
					for (int c=0; c < nbCols; c++) {
						int deltaRow = int.Parse (altitudeLineStr [charIdx]);
						int deltaCol = int.Parse (altitudeLineStr [charIdx + 1]);
						charIdx += 2;

						cazes [lineForGivenAltitude] [c].Winds [altitude] = new Vector(deltaRow, deltaCol);
					}
				}
			}

			return new Problem (nbLines, nbCols, nbAltitudes, rayonCouverture, nbToursSimu, departBallon, cazes, nbAvailableBallons);
		}
	}
}
