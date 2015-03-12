using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode
{
    class Solver
    {
        private readonly Problem problem;
        private readonly bool[,] dataCenter; // matrix representing sloting in dc : true means available, false filled
        private readonly List<ServerInSlot> slotedServer = new List<ServerInSlot>(); 


        public Solver(Problem problemStatement)
        {
            problem = problemStatement;
            // fill dataCenter with empty and unavailable slots
            dataCenter = new bool[problemStatement.NbRows, problemStatement.NbSlotsPerRows];
            for (int i = 0; i < problem.NbRows; i++)
                for (int j = 0; j < problem.NbSlotsPerRows; j++)
                    dataCenter[i,j] = problem.IsSlotAvailable(i, j);
        }


        public void Solve()
        {
            // -------------
            // Strategy
            // Assume that servers physical size ~ server capacity (wrong, but not slightly related to reality given the dataset)
            // Step 1 : fill the rows with servers, with priority to dispersing servers over many rows (stupid but heh)
            // Step 2 : set servers in groups to optimize capacity
            // -------------

            // Step 1 : place each servers in an available slot
            // todo : fill to balance over every row ! not fill rows first like its done now. (req !)
            // todo : randomize foreach, evaluate solution and submit the best one (good optimize approach)
            // todo : do not iterate over the whole matrix everytime (if time)
            foreach (var server in problem.Servers)
            {
                bool serverIsSloted = false;

                // find an empty slot for this servers
				for (int j = 0; j < problem.NbSlotsPerRows; j++)
                {
					for (int i = 0; i < problem.NbRows; i++)
                    {
                        if (serverIsSloted)
                            continue;

                        // check that the next [sizeofserver] slots are empty
                        bool slotable = true;
                     
                        for (int k = 0; k < server.Size; k++)
                        {
                            if (j + k >= problem.NbSlotsPerRows || !dataCenter[i, j + k]) // cannot slot here
                            {
                                slotable = false;
                                break;
                            }
                        }
                        if (slotable) // the slot is available !
                        {
                            slotedServer.Add(new ServerInSlot(server, -1, i, j)); // add our server in the "done" list
                            for (int k = 0; k < server.Size; k++) // set the space as unavailable in the dc
                                dataCenter[i,j + k] = false;
                            serverIsSloted = true;
                        }
                    }
                }
            }

            // Step 2 : servers are now in place. We will now assign groups to maximize availability
            






        }
    }
}
