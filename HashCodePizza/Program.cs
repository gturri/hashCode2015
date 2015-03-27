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
			var pizza = Parser.Read ();
			var solver = new GtSolver (pizza);
			solver.Dump (solver.Solve ());
			Console.ReadLine ();

            //var solver = new Solver();
            //solver.Solve(pizza);


        }
    }
}
