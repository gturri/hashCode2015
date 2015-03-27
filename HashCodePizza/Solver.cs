using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodePizza
{
    public class Solver
    {
        public void Solve(bool[,] pizza)
        {
            var solution = new List<Slice>();
            bool[,] cutSpace = new bool[180, 60];

            int jambonCounter = 0;
            int sliceSize = 0;
            int initialj = 0;
            bool resetSlice = false;

            for (int i = 0; i < pizza.GetLength(0); i++)
            {
                for (int j = 0; j < pizza.GetLength(1); j++)
                {
                    if (resetSlice)
                    {
                        jambonCounter = 0;
                        sliceSize = 0;
                        resetSlice = false;
                        initialj = j;
                    }

                    // Add this case
                    sliceSize++;
                    if (pizza[i, j])
                        jambonCounter++;

                    // cut slice if done
                    if (jambonCounter == 3)
                    {
                        if (sliceSize <= 12)
                        {
                            // We have a valid slice. add to solution and reset counter
                            solution.Add(new Slice(i, i, initialj, j));
                            for (int counter = initialj; counter < j; counter++)
                                cutSpace[i, counter] = true;

                            Console.WriteLine("slice !");


                            resetSlice = true;
                        }
                        else // reset with no solution :(
                        {
                            resetSlice = true;
                        }
                    }

                    if (j == pizza.GetLength(1) - 1)
                        resetSlice = true;



                }
            }
        }
    }
}
