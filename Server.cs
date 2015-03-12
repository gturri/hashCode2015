using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode
{
    public class Server
    {
        public int Capacity { get; private set; }
        public int Size { get; private set; }
        public int Idx { get; private set; }

        public Server(int capacity, int size, int idx)
        {
            Capacity = capacity;
            Size = size;
            Idx = idx;
        }

    }
}
