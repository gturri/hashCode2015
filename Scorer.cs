using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode
{
    class Scorer
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
    }
}
