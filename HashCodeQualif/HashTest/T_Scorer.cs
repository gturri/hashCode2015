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

        [Test]
        public void ComputeCorrectScore1()
        {
            List<ServerInSlot> solution = new List<ServerInSlot>();
            solution.Add(new ServerInSlot(new Server(capacity: 3, size: 1, idx: 0), groupId: 0, row: 0, col: 0));
            solution.Add(new ServerInSlot(new Server(capacity: 2, size: 1, idx: 1), groupId: 0, row: 1, col: 0));

            solution.Add(new ServerInSlot(new Server(capacity: 5, size: 1, idx: 2), groupId: 1, row: 0, col: 4));
            solution.Add(new ServerInSlot(new Server(capacity: 4, size: 1, idx: 3), groupId: 1, row: 1, col: 4));

            Assert.AreEqual(2, _scorer.ComputeScore(solution));
        }

        [Test]
        public void ComputeCorrectScore2()
        {
            List<ServerInSlot> solution = new List<ServerInSlot>();
            solution.Add(new ServerInSlot(new Server(capacity: 3, size: 1, idx: 0), groupId: 0, row: 0, col: 0));
            solution.Add(new ServerInSlot(new Server(capacity: 2, size: 1, idx: 1), groupId: 0, row: 1, col: 0));
            solution.Add(new ServerInSlot(new Server(capacity: 4, size: 1, idx: 2), groupId: 0, row: 1, col: 1));

            solution.Add(new ServerInSlot(new Server(capacity: 5, size: 1, idx: 3), groupId: 1, row: 0, col: 4));
            solution.Add(new ServerInSlot(new Server(capacity: 4, size: 1, idx: 4), groupId: 1, row: 1, col: 4));

            Assert.AreEqual(3, _scorer.ComputeScore(solution));
        }
    }
}
