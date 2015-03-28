using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodePizza
{
    public class Solver
    {
        private static string BuildFilename()
        {
            DateTime now = DateTime.Now;
            return now.Hour + "_" + now.Minute + "_" + now.Second + ".solution.txt";
        }

        public void Solve(bool[,] pizza)
        {
            var solution = new List<Slice>();
            bool[,] cutSpace = new bool[180, 60];
            string filename = BuildFilename();
            List<string> textSolution = new List<string>();

            int totalSliceCounter = 0;


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
                        
                        var slice = new Slice();
                        int jambonCounter = 0;
                        int sliceSize = 0;
                            totalSliceCounter++;
                            textSolution.Add(i + " " + initialj + " " + i + " " + j);
                            Console.WriteLine(i + " " + initialj + " " + i + " " + j);

                            resetSlice = true;
                        }
                        else // move initial slice once to the right
                        {
                            /*initialj++;
                            sliceSize--;*/
                            resetSlice = true;
                        }
                    }

                    if (j == pizza.GetLength(1) - 1)
                        resetSlice = true;



                }
            }

            // new round to fill the blanks !
           /* jambonCounter = 0;
            sliceSize = 0;
            initialj = 0;
            resetSlice = false;

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
                    if (cutSpace[i, j])
                    {
                        resetSlice = true;
                        continue;
                    }

                }
            }*/


            Console.WriteLine(totalSliceCounter.ToString());

            var realResultToOutput = new List<String>();
            realResultToOutput.Add(totalSliceCounter.ToString());
            foreach (var line in textSolution)
            {
                realResultToOutput.Add(line);
            }

            File.WriteAllLines(BuildFilename(), realResultToOutput);

        }
    }
}
