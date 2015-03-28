using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace finale
{
    public class MapPrinter
    {
        public void PrintMap(Problem problem)
        {
            for (int row = 0; row < problem.NbLines; row++)
            {
                for (int col = 0; col < problem.NbCols; col++)
                {
                    bool target = false;
                    foreach (var localisation in problem.Targets)
                        if (localisation.Col == col && localisation.Line == row)
                            target = true;

                    if (target)
                        Console.Write("x");
                    else
                        Console.Write(".");

                        
                }
                Console.Write("\n");
            }


            Console.WriteLine("press key to continue...");
            Console.ReadKey();
        }


    }
}
