namespace finale
{
    static class DebugHelper
    {
        public static int NbCovered(byte[,] covered)
        {
            int n = 0;
            for (int i = 0; i < covered.GetLength(0); i++)
                for (int j = 0; j < covered.GetLength(1); j++)
                    n += covered[i, j];
            return n;
        }
    }
}