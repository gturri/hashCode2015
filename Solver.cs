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
        private List<ServerInSlot> slotedServers = new List<ServerInSlot>(); 


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
			int unslotted = 0;
			int capaLost = 0;
            foreach (var server in problem.Servers)
            {
                bool serverIsSloted = false;

                // try : remove all servers where capacity/size < 10
                if ((server.Size == 5 && server.Capacity < 40 )
                 || (server.Size == 4 && server.Capacity < 20)
                     || server.Capacity < 15)
                {
                    unslotted++;
                    capaLost += server.Capacity;
                    slotedServers.Add(new ServerInSlot(server, -1, -1, -1));
                    Console.WriteLine("ignored server of size {0} with capa {1}", server.Size, server.Capacity);
                    continue;
                }
                // end try

                // find an empty slot for this servers
				for (int j = 0; j < problem.NbSlotsPerRows - server.Size; j++)
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
                            slotedServers.Add(new ServerInSlot(server, -1, i, j)); // add our server in the "done" list
                            for (int k = 0; k < server.Size; k++) // set the space as unavailable in the dc
                                dataCenter[i,j + k] = false;
                            serverIsSloted = true;
                            Console.WriteLine("Server solted");
                        }
                    }
                }

				if (!serverIsSloted)
				{
					Console.WriteLine ("cannot slot server of size {0} with capa {1}", server.Size, server.Capacity);
					unslotted++;
					capaLost += server.Capacity;
                    slotedServers.Add(new ServerInSlot(server, -1, -1, -1));
				}
            }
			Console.WriteLine ("{0} servers left behind, for a capa of {1}", unslotted, capaLost);
			int remaining = 0;
			foreach (var b in dataCenter)
				if (b) remaining++;
			Console.WriteLine (remaining + " slots remaining in dc");


            var newSlotedServers = new List<ServerInSlot>(); 
            foreach (var slotedServer in slotedServers)
            {
                if (slotedServer.IdxCol == -1) // not sloted
                {
                    bool serverIsSloted = false;
                    // find an empty slot for this servers
                    for (int j = 0; j < problem.NbSlotsPerRows - slotedServer.Server.Size; j++)
                    {
                        for (int i = 0; i < problem.NbRows; i++)
                        {
                            if (serverIsSloted)
                                continue;

                            // check that the next [sizeofserver] slots are empty
                            bool slotable = true;

                            for (int k = 0; k <  slotedServer.Server.Size; k++)
                            {
                                if (j + k >= problem.NbSlotsPerRows || !dataCenter[i, j + k]) // cannot slot here
                                {
                                    slotable = false;
                                    break;
                                }
                            }
                            if (slotable) // the slot is available !
                            {
                                newSlotedServers.Add(new ServerInSlot(slotedServer.Server, -1, i, j)); // add our server in the "done" list
                                for (int k = 0; k < slotedServer.Server.Size; k++) // set the space as unavailable in the dc
                                    dataCenter[i, j + k] = false;
                                serverIsSloted = true;
                                Console.WriteLine("Server solted");
                            }
                        }
                    }
                    if (!serverIsSloted)
                    {
                        Console.WriteLine("not sloted on second try : size {0} with capa {1}", slotedServer.Server.Size, slotedServer.Server.Capacity);

                        newSlotedServers.Add(new ServerInSlot(slotedServer.Server, -1, -1, -1));
                    }
                }
                else
                {
                    newSlotedServers.Add(slotedServer);
                }
            }


            slotedServers = newSlotedServers;

            Console.WriteLine("Step 2");
            // Step 2 : servers are now in place. Now assign groups to maximize availability
            // easy solution : round robin
			var groupCapas = new int[problem.NbGroupsToBuild];
            int group = 0;
            foreach (var serverInSlot in slotedServers)
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
				//Console.WriteLine ("g{0}\t{1}", i, item);
			}
			Console.WriteLine ("smallest:{0}", groupCapas.Min());
			Console.WriteLine ("biggest:{0}", groupCapas.Max());
            Console.WriteLine("Done !");
            return slotedServers;
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
