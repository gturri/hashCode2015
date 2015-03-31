using System;
using System.Collections.Generic;

namespace finale
{
	public class Solution
	{
        /// <summary> first index is turn, second is balloon </summary>
		public int[,] Moves = new int[400, 53];

		public int currentTurn = 0;
		public Problem problem { get; private set; }

		public Solution (Problem problem)
		{
			this.problem = problem;
		}

		public void RegisterBaloonMove(int ballonIdx, int move)
		{
			System.Diagnostics.Debug.Assert (move < 2 && move > -2);
			System.Diagnostics.Debug.Assert (ballonIdx >= 0 && ballonIdx < 53);

			Moves [currentTurn,ballonIdx] = move;
		}
	}
}
