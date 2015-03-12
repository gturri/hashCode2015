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
         //   for (int unavailableRowIdx = 0; unavailableRowIdx < _problem.NbRows; unavailableRowIdx++)
         //   {
         //       score = Math.Min(score, ComputeScore(solution, unavailableRowIdx));
         //   }
            for (int groupId = 0; groupId < _problem.NbGroupsToBuild; groupId ++)
            {
                score = Math.Min(score, ComputeGroupCapacity(groupId, solution));
            }
                return score;
        }

        public int GetPoolIdxWithLowestCapacity(List<ServerInSlot> solution)
        {
            int minCapacity = int.MaxValue;
            int idxWorstGroup = -1;

            for (int groupIdx = 0; groupIdx < _problem.NbGroupsToBuild; groupIdx++)
            {
                int groupReservedCapacity = ComputeGroupCapacity(groupIdx, solution);
                if (minCapacity > groupReservedCapacity)
                {
                    minCapacity = groupReservedCapacity;
                    idxWorstGroup = groupIdx;
                }
            }

            if (idxWorstGroup == -1)
            {
                throw new Exception("Shouldn't return -1");
            }
            return idxWorstGroup;
        }

        public int GetPoolIdxWithBestCapacity(List<ServerInSlot> solution)
        {
            int maxCapacity = int.MinValue;
            int idxBestGroup = -1;

            for (int groupIdx = 0; groupIdx < _problem.NbGroupsToBuild; groupIdx++)
            {
                int groupReservedCapacity = ComputeGroupCapacity(groupIdx, solution);
                if (maxCapacity < groupReservedCapacity)
                {
                    maxCapacity = groupReservedCapacity;
                    idxBestGroup = groupIdx;
                }
            }

            if (idxBestGroup == -1)
            {
                throw new Exception("Shouldn't return -1");
            }
            return idxBestGroup;
        }

        private int ComputeGroupCapacity(int groupIdx, List<ServerInSlot> solution)
        {
            int worstRowScore = int.MaxValue;
            for (int idxRow = 0; idxRow < _problem.NbRows; idxRow++)
            {
                int score = 0;
                for (int s = 0; s < solution.Count; s++)
                {
                    var server = solution[s];
                    if (server.IdxRow != idxRow && server.Group == groupIdx)
                    {
                        score += server.Server.Capacity;
                    }
                }
                worstRowScore = Math.Min(worstRowScore, score);
            }
            return worstRowScore;
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
