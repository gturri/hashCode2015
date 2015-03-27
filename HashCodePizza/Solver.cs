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
            bool[,] cutSpace = new bool[,];


            for (int i = 0; i < pizza.GetLength(0); i++)
                {
                    for (int j = 0; j < pizza.GetLength(1); j++)
                    {             
                        
                        var slice = new Slice();
                        int jambonCounter = 0;
                        int sliceSize = 0;

                        // Add this case
                        sliceSize++;
                        if (pizza[i,j])
                            jambonCounter++;

                        // cut slice if done
                        if (jambonCounter == 3)
                        {
                            if (sliceSize <= 12)
                            {
                                // We have a valid slice.
                            }
                        }



            }

        }
    }
}
