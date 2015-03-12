using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace HashCode
{
    class ServerInSlot
    {
        private Server server;
        private int group;
        private int IdxRow;
        private int IdxCol;

        public ServerInSlot(Server serv, int groupId, int row, int col)
        {
            server = serv;
            group = groupId;
            IdxRow = row;
            IdxCol = col;
        }


    }
}
