using System;
using System.Collections.Generic;
using System.IO;

namespace finale
{
	public class Dumper
	{
		public static void Dump (Solution solution, string destFile = null)
		{
			List<string> strSoluce = new List<String> ();
			for ( int tour = 0 ; tour < solution.problem.NbTours ; tour++ ){
				string line = String.Empty;
				for (int ballon=0; ballon < solution.problem.NbAvailableBallons; ballon++) {
					line += solution.Moves [tour][ballon] + ' ';
				}
				strSoluce.Add(line);
			}

			File.WriteAllLines (destFile ?? BuildFilename(), strSoluce);
		}

		private static string BuildFilename()
		{
			DateTime now = DateTime.Now;
			return now.Hour + "_" + now.Minute + "_" + now.Second + ".solution.txt";
		}
	}
}
