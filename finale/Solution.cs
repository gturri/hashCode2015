using System;
using System.Collections.Generic;

namespace finale
{
	public class Solution
	{
        /// <summary> first index is turn, second is balloon </summary>
		public int[,] Moves = new int[400, 53];

		public Problem problem { get; private set; }

		public Solution (Problem problem)
		{
			this.problem = problem;
		}
	}
}
