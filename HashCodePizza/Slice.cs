using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodePizza
{
    public class Slice
    {
        public int Initiali { get; set; }
        public int Finali { get; set; }
        public int Initialj { get; set; }
        public int Finalj { get; set; }

        public Slice(int initiali, int finali, int initialj, int finalj)
        {
            Initiali = initiali;
            Finali = finali;
            Initialj = initialj;
            Finalj = finalj;
        }
    }
}
