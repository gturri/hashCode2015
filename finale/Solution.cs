using System;
using System.Collections.Generic;
using System.Text;

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

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < 53; j++)
                {
                    switch (Moves[i, j])
                    {
                        case -1:
                            sb.Append('v');
                            break;
                        case 0:
                            sb.Append('-');
                            break;
                        case 1:
                            sb.Append('^');
                            break;
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
	}
}
