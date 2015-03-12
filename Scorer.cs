using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode
{
    public class Scorer
    {
        private readonly Problem _problem;

        public Scorer(Problem problem)
        {
            _problem = problem;
        }

        public bool IsValid(List<ServerInSlot> solution)
        {
            for (int i = 0; i < solution.Count(); i++)
            {
                ServerInSlot server = solution[i];

                int rowIdx = server.IdxRow;
                int colIdx = server.IdxCol;

                for (int j = 0; j < server.Server.Size; j++)
                {
                    if (!_problem.IsSlotAvailable(rowIdx, colIdx + j))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public int ComputeScore(List<ServerInSlot> solution)
        {
            int score = int.MaxValue;
            for (int unavailableRowIdx = 0; unavailableRowIdx < _problem.NbRows; unavailableRowIdx++)
            {
                score = Math.Min(score, ComputeScore(solution, unavailableRowIdx));
            }
            return score;
        }

        private int ComputeScore(List<ServerInSlot> solution, int unavailableRowIdx)
        {
            List<int> scorePerGroup = new List<int>();
            for (int i = 0; i < _problem.NbGroupsToBuild; i++)
            {
                scorePerGroup.Add(0);
            }

            for (int idxServer = 0; idxServer < solution.Count; idxServer++)
            {
                var server = solution[idxServer];
                if (server.IdxRow != unavailableRowIdx)
                {
                    scorePerGroup[server.Group] += server.Server.Capacity;
                }
            }
            return scorePerGroup.Min();

        }
    }
}
