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
            short nbLines = short.Parse(firstLine[0]);
            short nbCols = short.Parse(firstLine[1]);
            short nbAltitudes = short.Parse(firstLine[2]);

			//2nd line
			string[] secondLine = text [1].Split (' ');
            short nbCibles = short.Parse(secondLine[0]);
            short rayonCouverture = short.Parse(secondLine[1]);
            short nbAvailableBallons = short.Parse(secondLine[2]);
            short nbToursSimu = short.Parse(secondLine[3]);

			//3rd line
			string[] thirdLine = text [2].Split (' ');
			Vec2 departBallon = new Vec2 (short.Parse (thirdLine [0]), short.Parse (thirdLine [1]));

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
					string[] altitudeLineStr = text [line].Split (' ');
					line++;

				
					int charIdx = 0;
					for (int c=0; c < nbCols; c++) {
                        short deltaRow = short.Parse(altitudeLineStr[charIdx]);
                        short deltaCol = short.Parse(altitudeLineStr[charIdx + 1]);
						charIdx += 2;

						cazes [lineForGivenAltitude] [c].Winds [altitude] = new Vec2(deltaRow, deltaCol);
					}
				}
			}

			return new Problem (nbLines, nbCols, nbAltitudes, rayonCouverture, nbToursSimu, departBallon, cazes, nbAvailableBallons);
		}
	}
}
