using System.IO;
using System;

namespace finale
{
    static class Helper
    {
        public static int NbCovered(byte[,] covered)
        {
            int n = 0;
            for (int i = 0; i < covered.GetLength(0); i++)
                for (int j = 0; j < covered.GetLength(1); j++)
                    n += covered[i, j];
            return n;
        }

		/// <summary>
		/// Shuffle the order of the balloons in the solution
		/// </summary>
		public static void Shuffle(Solution s)
		{
			for (int b = 0; b < 52; b++)
				SwapLines (s, b, MainClass.rand.Next (b + 1, 53));
		}

		private static void SwapLines(Solution s, int b1, int b2)
		{
			for(int t = 0; t < 400; t++)
			{
				int tmp = s.Moves [t, b1];
				s.Moves [t, b1] = s.Moves [t, b2];
				s.Moves [t, b2] = tmp;
			}
		}

		public static string GetBestSolutionFromFolder(string folder)
		{
			int max = 0;
			foreach (var file in Directory.EnumerateFiles(folder))
			{
				if (!file.EndsWith (".txt"))
					continue;
				var fileName = Path.GetFileNameWithoutExtension (file);
				int score;
				if (!Int32.TryParse (fileName, out score))
					continue;
				if (score > max)
					max = score;
			}
			if (max > 0)
				return max + ".txt";
			else
				return null;
		}
    }
}