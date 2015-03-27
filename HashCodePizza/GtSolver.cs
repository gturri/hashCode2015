using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HashCodePizza
{
	public class GtSlice {
		public int l1, l2, c1, c2;

		public GtSlice(int l1, int l2, int c1, int c2){
			this.l1 = l1;
			this.l2 = l2;
			this.c1 = c1;
			this.c2 = c2;
		}

		public override string ToString ()
		{
			return l1 + " " + c1 + " " + l2 + " " + c2;
		}
	}

	public class GtSolver
	{
			private bool[,] _pizza;

			private bool[,] _used;

			public GtSolver (bool[,] pizza)
			{
				_pizza = pizza;
				_used = new bool[Parser.nbLines, Parser.nbCols];
			}

			public void Dump(List<GtSlice> sol){
			List<string> textSol = new List<string> ();
			textSol.Add (sol.Count.ToString ());
			foreach (var slice in sol) {
				textSol.Add (slice.ToString());
			}
			File.WriteAllLines("gtSol", textSol);
			}

			public List<GtSlice> Solve(){
				List<GtSlice> result = new List<GtSlice> ();

				int l = -1, c = -1;
				do {
					Tuple<int, int> nextCaze = FindNextCaze (l, c);
				l = nextCaze.Item1;
				c = nextCaze.Item2;
				Console.WriteLine("l = " + l + ", c = " + c);
					var possibleSlices = GetPossibleSlices(l, c);
					if ( possibleSlices.Count > 0 ){
					var slice = ChooseSlice(possibleSlices);
						result.Add(slice);
						MarkAsUsed(slice);
					}
				} while ( l < Parser.nbLines );
				return result;
			}

		GtSlice ChooseSlice(List<GtSlice> slices){
			GtSlice result = null;
			int maxSize = -1;
			foreach (var slice in slices) {
				int size = Size (slice);
				if (size > maxSize) {
					maxSize = size;
					result = slice;
				}
			}
			return result;
		}

			private Tuple<int, int> FindNextCaze(int l, int c) {
				if (c == -1) {
					return new Tuple<int, int> (0, 0);
				}
			if (c < Parser.nbCols - 1) {
				c++;
			} else {
				l++;
				c = 0;
			}

			while (l < Parser.nbLines && _used[l, c]) {
					if (c != Parser.nbCols - 1) {
						c++;
					} else {
						l++;
						c = 0;
					}
				}

				return new Tuple<int, int> (l, c);
			}

			private List<GtSlice> GetPossibleSlices(int l, int c){
				List<GtSlice> result = new List<GtSlice> ();
				foreach (Tuple<int, int> format in AllFormats) {
					if (l + format.Item1 >= Parser.nbLines || c + format.Item2 >= Parser.nbCols ) {
						continue;
					}
					GtSlice potentialSlice = new GtSlice (l, l + format.Item1 - 1, c, c + format.Item2 - 1);

					if (CountHam(potentialSlice) < Parser.hamMin) {
						continue;
					}

					if (!CazeAvailables (potentialSlice)) {
					continue;
					}

					result.Add (potentialSlice);
				}
				return result;
			}

			private bool CazeAvailables(GtSlice slice){
				for (int l = slice.l1; l <= slice.l2; l++) {
					for (int c = slice.c1; c <= slice.c2; c++) {
						if (_used [l, c]) {
							return false;
						}
					}
				}
				return true;
			}

		private void MarkAsUsed(GtSlice slice){
			for (int l = slice.l1; l <= slice.l2; l++) {
				for (int c = slice.c1; c <= slice.c2; c++) {
					_used [l, c] = true;
				}
			}
		}

		private int Size(GtSlice slice){
			return (slice.l2 - slice.l1 + 1) * (slice.c2 - slice.c1 + 1);
		}

			private int CountHam(GtSlice slice) {
				int count = 0;
				for (int l = slice.l1; l <= slice.l2; l++) {
					for (int c = slice.c1; c <= slice.c2; c++) {
						if (_pizza [l, c]) {
							count++;
						}
					}
				}
				return count;
			}

			private static List<Tuple<int, int>> AllPossibleFormat(){
				List<Tuple<int, int>> result = new List<Tuple<int, int>> ();
				for (int nbCols = 1; nbCols <= 12; nbCols ++) {
					for (int nbL = 1; nbL <= 12; nbL ++) {
						if (nbCols * nbL <= 12) {
							result.Add (new Tuple<int, int> (nbCols, nbL));
						}
					}
				}
				return result;
			}

			private static List<Tuple<int, int>> AllFormats = AllPossibleFormat();
		}
}

