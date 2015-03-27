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


        public Solver(Problem problemStatement)
        {
            problem = problemStatement;
            // fill dataCenter with empty and unavailable slots
            dataCenter = new bool[problemStatement.NbRows, problemStatement.NbSlotsPerRows];
            for (int i = 0; i < problem.NbRows; i++)
                for (int j = 0; j < problem.NbSlotsPerRows; j++)
                    dataCenter[i,j] = problem.IsSlotAvailable(i, j);
        }


        public List<ServerInSlot> Solve()
        {
            // -------------
            // Strategy
            // Assume that servers physical size ~ server capacity (wrong, but not slightly related to reality given the dataset)
            // Step 1 : fill the rows with servers, with priority to dispersing servers over many rows (stupid but heh)
            // Step 2 : set servers in groups to optimize capacity
            // -------------

            
            Console.WriteLine("Step 1");
            // Step 1 : place each servers in an available slot
            // todo : fill to balance over every row ! not fill rows first like its done now. (req !)
            // todo : randomize foreach, evaluate solution and submit the best one (good optimize approach)
            // todo : do not iterate over the whole matrix everytime (if time)
            int slotingStep = 1;

            // init the loop with problem statement
            var finalSlotedServers = new List<ServerInSlot>();
            foreach (var server in problem.Servers)
                finalSlotedServers.Add(new ServerInSlot(server, -1, -1, -1));

            while (true)
            {
                int unslotted = 0;
                int capaLost = 0;
                int sloted = 0;
                var slotedServersInThisIteration = new List<ServerInSlot>(); 
                foreach (var serverInSlot in finalSlotedServers)
                {
                    bool serverIsSloted = false;
                    var server = serverInSlot.Server;
              
                    // Skip if already sloted in previous iteration
                    if (serverInSlot.IdxCol != -1)
                    {
                        sloted++;
                        slotedServersInThisIteration.Add(serverInSlot);
                        continue;
                    }
    

                    // Slot best servers first
                    if ( (server.Capacity /server.Size) <= (30 - slotingStep))
                    {
                        unslotted++;
                        capaLost += server.Capacity;
                        slotedServersInThisIteration.Add(new ServerInSlot(server, -1, -1, -1));
                        //Console.WriteLine("ignored server of size {0} with capa {1}", server.Size, server.Capacity);
                        continue;
                    }
    

                    // find an empty slot for this server
				    for (int j = 0; j <= problem.NbSlotsPerRows - server.Size; j++)
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
                                slotedServersInThisIteration.Add(new ServerInSlot(server, -1, i, j)); // add our server in the "done" list
                                for (int k = 0; k < server.Size; k++) // set the space as unavailable in the dc
                                    dataCenter[i,j + k] = false;
                                serverIsSloted = true;
                                sloted++;
                                //Console.WriteLine("Server solted");
                            }
                        }
                    }

				    if (!serverIsSloted)
				    {
					    //Console.WriteLine ("cannot slot server of size {0} with capa {1}", server.Size, server.Capacity);
					    unslotted++;
					    capaLost += server.Capacity;
                        slotedServersInThisIteration.Add(new ServerInSlot(server, -1, -1, -1));
				    }
                }


                Console.WriteLine("---------------");
                Console.WriteLine("Report of sloting step " + slotingStep);
			    Console.WriteLine ("{0} servers left behind, for a capa of {1}", unslotted, capaLost);
                int remainingSlots = 0;
			    foreach (var b in dataCenter)
                    if (b) remainingSlots++;
                Console.WriteLine(remainingSlots + " slots remaining in dc");

                finalSlotedServers = slotedServersInThisIteration;
                slotingStep++;
                if (slotingStep > 32)
                    break;
            }

            Console.WriteLine("--- Sloting done ---");
            foreach (var finalSlotedServer in finalSlotedServers)
            {
                if (finalSlotedServer.IdxCol == -1)
                    Console.WriteLine("Left : size {0} capa {1}", finalSlotedServer.Server.Size, finalSlotedServer.Server.Capacity);
            }
            Console.ReadKey();

            Console.WriteLine("Step 2");
            // Step 2 : servers are now in place. Now assign groups to maximize availability
            // easy solution : round robin
			var groupCapas = new int[problem.NbGroupsToBuild];
            int group = 0;
            foreach (var serverInSlot in finalSlotedServers)
            {
                if (serverInSlot.IdxCol == -1)
                    continue;

				int target = group % problem.NbGroupsToBuild;
                serverInSlot.Group = target;
				groupCapas [target] += serverInSlot.Server.Capacity;
                group++;
            }

			for (int i = 0; i < groupCapas.Length; i++)
			{
				var item = groupCapas [i];
				Console.WriteLine ("g{0}\t{1}", i, item);
			}
			Console.WriteLine ("Smallest:{0}", groupCapas.Min());
			Console.WriteLine ("Biggest:{0}", groupCapas.Max());
            Console.WriteLine("Done !");
            Console.ReadKey();

            return finalSlotedServers;
        }
    }
   /* public class ServerCapacityComparer : IComparer<ServerInSlot>
    {
        public int Compare(ServerInSlot x, ServerInSlot y)
        {
            return x.Server.Capacity.CompareTo(y.Server.Capacity);
        }
    }*/
}
