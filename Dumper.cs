using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace HashCode
{
    public class Dumper
    {
        public static void Dump(List<ServerInSlot> solution)
        {
            string filename = BuildFilename();

            List<ServerInSlot> sortedSolution = SortSolution(solution);
            List<string> textSolution = new List<string>();
            foreach (var server in sortedSolution)
            {
                textSolution.Add(server.ToSolutionString());
            }

            File.WriteAllLines(filename, textSolution);
        }

        private static string BuildFilename()
        {
            DateTime now = DateTime.Now;
            return now.Hour + "_" + now.Minute + "_" + now.Second + ".solution.txt";
        }

        private static List<ServerInSlot> SortSolution(IEnumerable<ServerInSlot> solution)
        {
            List<ServerInSlot> copy = new List<ServerInSlot>(solution);
            copy.Sort(new ServerInSlotComparer());
            return copy;
        }

        public class ServerInSlotComparer : IComparer<ServerInSlot>
        {
            public int Compare(ServerInSlot x, ServerInSlot y)
            {
                return x.Server.Idx.CompareTo(y.Server.Idx);
            }
        }
    }
}
