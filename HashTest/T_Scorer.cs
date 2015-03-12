using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashCode;
using NUnit.Framework;

namespace HashTest
{
    public class T_Scorer
    {
        private Scorer _scorer;

        [SetUp]
        public void SetUp()
        {
            var problem = new Problem(Path.Combine("..", "..", "..", "testFiles", "petProblem.in"));
            _scorer = new Scorer(problem);
        }

        [Test]
        public void ShoudRejectInvalidSolutionWithServerOfSizeOne()
        {
            List<ServerInSlot> solution = new List<ServerInSlot>();
            solution.Add(new ServerInSlot(new Server(1, 1, 1), 0, 0, 1));
            Assert.False(_scorer.IsValid(solution));
        }

        [Test]
        public void ShouldRejectInvalidSolutionWithServerOfSizeTwo()
        {
            List<ServerInSlot> solution = new List<ServerInSlot>();
            solution.Add(new ServerInSlot(new Server(1, 2, 1), 0, 0, 0));
            Assert.False(_scorer.IsValid(solution));
        }

        [Test]
        public void ShouldAcceptValidSolution()
        {
            List<ServerInSlot> solution = new List<ServerInSlot>();
            solution.Add(new ServerInSlot(new Server(1, 1, 1), 0, 0, 2));
            Assert.True(_scorer.IsValid(solution));
        }
    }
}
