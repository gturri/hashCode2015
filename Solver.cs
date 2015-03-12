using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode
{
    class Solver
    {
        private readonly Problem problem;
        private List<List<Slot>> dataCenter;

        public Solver(Problem problemStatement)
        {
            problem = problemStatement;
        }


        public void Solve()
        {
            dataCenter = new List<List<Slot>>();
            // fill dataCenter with empty and unavailable slots
            for (int i = 0; i < problem.NbRows; i++)
                for (int j = 0; j < problem.SlotsPerRows; j++)
                    dataCenter[i][j] = problem.IsSlotAvailable(i, j) ? new Slot(i, j, false) : new Slot(i, j, true);


                    





        }
    }
}
