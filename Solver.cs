using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode
{
    class Solver
    {
        private readonly Problem problem;
        private readonly List<List<Slot>> dataCenter = new List<List<Slot>>();
        private readonly List<ServerInSlot> slotedServer = new List<ServerInSlot>(); 


        public Solver(Problem problemStatement)
        {
            problem = problemStatement;
        }


        public void Solve()
        {
            // fill dataCenter with empty and unavailable slots
            for (int i = 0; i < problem.NbRows; i++)
                for (int j = 0; j < problem.NbSlotsPerRows; j++)
                    dataCenter[i][j] = problem.IsSlotAvailable(i, j) ? new Slot(i, j, false) : new Slot(i, j, true);



            // Filling Strategy
            // Assume that servers physical size ~ server capacity (wrong, but not slightly related to reality given the dataset)
            // Step 1 : fill the rows with servers, with priority to dispersing servers over many rows (stupid but heh)
            // Step 2 : set servers in groups to optimize capacity


            // Step 1 : place each servers in an available slot
            foreach (var server in problem.Servers)
            {
                int serverRow = -1;
                int serverCol = -1;

                // find an empty slot
                foreach (var nbRow in dataCenter)
                {
                    
                }

                // update dc status
                // update server status
                slotedServer.Add(new ServerInSlot(server, -1, serverRow, serverCol));
            }

            // Step 2 : servers are now in place. We will now select groups to maximize availability
                    





        }
    }
}
