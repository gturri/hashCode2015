using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace HashCode
{
    public class ServerInSlot
    {
        public Server Server { get; private set; }
        public int Group { get; private set; }
        public int IdxRow { get; private set; }
        public int IdxCol { get; private set; }

        public ServerInSlot(Server serv, int groupId, int row, int col)
        {
            Server = serv;
            Group = groupId;
            IdxRow = row;
            IdxCol = col;
        }


        public string ToSolutionString()
        {
            return IdxRow + " " + IdxCol + " " + Group;
        }
    }
}
