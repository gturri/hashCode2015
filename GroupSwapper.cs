using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode
{
    class GroupSwapper
    {
        private readonly Scorer _scorer;
        private readonly int _nbGroups;
        private readonly List<ServerInSlot> _initialSolution;
        private int _bestScore = 0;
        private readonly Random _random = new Random();

        public GroupSwapper(Scorer scorer, List<ServerInSlot> initialSolution, int nbGroups)
        {
            _scorer = scorer;
            _initialSolution = initialSolution;
            _nbGroups = nbGroups;
        }

        public void Run()
        {
            while (true)
            {
                var newSolution = new List<ServerInSlot>(_initialSolution);
                for (int idxServer = 0; idxServer < newSolution.Count; idxServer++)
                {
                    newSolution[idxServer].Group = _random.Next(0, _nbGroups);
                }

                int newScore = _scorer.ComputeScore(newSolution);
                Console.WriteLine("Score is: " + newScore + ", best so far: " + _bestScore);
                if (newScore > _bestScore)
                {
                    _bestScore = newScore;
                    Dumper.Dump(newSolution);
                }
            }
        }

        private void Swap(List<ServerInSlot> currentSolution)
        {
            throw new NotImplementedException();
        }
    }
}
