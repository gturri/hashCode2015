using System;
using System.Collections.Generic;

namespace finale
{
	public class Solution
	{
		public List<int[]> Moves = new List<int[]>(400);

		public int currentTurn = 0;
		public Problem problem { get; private set; }

		public Localisation[] BaloonsPositions = new Localisation[53];
		public int[] BaloonsAltitude = new int[53];

		public List<List<Localisation>> BallonsAtEndOfEachTurn = new List<List<Localisation>> ();

		public Solution (Problem problem)
		{
			this.problem = problem;
			Moves [0] = new int[53];

			for (int i = 0; i < 53; i++)
			{
				BaloonsPositions [i] = problem.DepartBallons;
			}
		}

		public void RegisterBaloonMove(int ballonIdx, int move)
		{
			System.Diagnostics.Debug.Assert (move < 2 && move > -2);
			System.Diagnostics.Debug.Assert (ballonIdx > 0 && ballonIdx < 53);

			Moves [currentTurn] [ballonIdx] = move;


		}
	}
}
