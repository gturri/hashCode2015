using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode
{
    class Problem
    {
        public int NbRows { get; private set; }
        public int SlotsPerRows { get; private set; }
        public int NbSlotsUnavailable { get; private set; }
        public int NbGroupsToBuild { get; private set; }

        public IList<Server> Servers { get; private set; }
        public

        public Problem(int nbRows, int slotsPerRows, int nbSlotsUnavailable, int nbGroupsToBuild, IList<Server> servers)
        {
            NbRows = nbRows;
            SlotsPerRows = slotsPerRows;
            NbSlotsUnavailable = nbSlotsUnavailable;
            NbGroupsToBuild = nbGroupsToBuild;
            Servers = servers;
        }

        public bool IsSlotAvailable(int idxRow, int idxCol)
        {
            //TODO
            return false;
        }
    }
}
