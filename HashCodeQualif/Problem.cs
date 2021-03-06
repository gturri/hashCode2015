﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace HashCode
{
    public class Problem
    {
        public int NbRows { get; private set; }
        public int NbSlotsPerRows { get; private set; }
        public int NbSlotsUnavailable { get; private set; }
        public int NbGroupsToBuild { get; private set; }

        public int NbServers { get; private set; }
        public IList<Server> Servers { get; private set; }

        public List<List<bool>> SlotsAvailable;

        public Problem()
            :this(Path.Combine("..", "..", "dc.in"))
        {}

        public Problem(string file)
        {
            string[] text = File.ReadAllLines(file);

            string[] firstLine = text[0].Split(' ');


            //TODO
            NbRows = int.Parse(firstLine[0]);
            NbSlotsPerRows = int.Parse(firstLine[1]);
            NbSlotsUnavailable = int.Parse(firstLine[2]);
            NbGroupsToBuild = int.Parse(firstLine[3]);
            NbServers = int.Parse(firstLine[4]);

            InitializeSlots(text);
            InitializeServers(text);
        }

        private void InitializeSlots(String[] text)
        {
            SlotsAvailable = new List<List<bool>>();
            for (int idxRow = 0; idxRow < NbRows; idxRow++)
            {
                SlotsAvailable.Add(new List<bool>());
                for (int idxCol = 0; idxCol < NbSlotsPerRows; idxCol++)
                {
                    SlotsAvailable[idxRow].Add(true);
                }
            }

            for (int i = 1; i < NbSlotsUnavailable+1; i++)
            {
                string[] textSlot = text[i].Split(' ');
                SlotsAvailable[int.Parse(textSlot[0])][int.Parse(textSlot[1])] = false;
            }
        }

        private void InitializeServers(String[] text)
        {
            Servers = new List<Server>();
            int counter = 0;
            for (int i = 1 + NbSlotsUnavailable; i < 1 + NbSlotsUnavailable + NbServers; i++ )
            {
                String[] textServer = text[i].Split(' ');
                Server server = new Server(int.Parse(textServer[1]), int.Parse(textServer[0]), counter);
                counter++;
                Servers.Add(server);
            }

			//no order = loose 4784 capa
			//2376 with order by throuput
			//2356 by putting small servers last
			Servers = Servers.OrderByDescending (s => (s.Capacity/s.Size)*10 + s.Size/*larger servers first*/).ToList();
        }

        public bool IsSlotAvailable(int idxRow, int idxCol)
        {
            return SlotsAvailable[idxRow][idxCol];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < NbRows; i++)
            {
                for (int j = 0; j < NbSlotsPerRows; j++)
                {
                    char c = IsSlotAvailable(i, j) ? '-' : 'x';
                    sb.Append(c);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
