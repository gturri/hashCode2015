using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodePizza
{
    class Program
    {
        static void Main(string[] args)
        {
            // true : jambon
            // false : le reste
            var pizza = new List<List<bool>>(); // rempli par guillaume


            var solver = new Solver();
            solver.Solve(pizza);


        }
    }
}
