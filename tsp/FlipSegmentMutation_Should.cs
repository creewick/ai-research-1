using System.Drawing;
using System.Linq;
using NUnit.Framework;

namespace AiAlgorithms.Tsp
{
    [TestFixture]
    public class FlipSegmentMutation_Should
    {
        private static readonly Point[] checkpoints =
            {new Point(0, 0), new Point(10, 0), new Point(20, 10), new Point(5, 8), new Point(20, 15)};

        [TestCase(0, 2, new[] {1, 0, 2, 3})]
        [TestCase(0, 3, new[] {2, 1, 0, 3})]
        [TestCase(3, 2, new[] {3, 1, 2, 0})]
        public void GetResult(int start, int len, int[] expectedOrder)
        {
            var mutation = CreateMutation(expectedOrder.Length, start, len);
            var order = mutation.GetResult().Order;
            Assert.AreEqual(expectedOrder, order);
        }

        [TestCase(4, 0, 2)]
        [TestCase(4, 1, 2)]
        [TestCase(4, 3, 2)]
        [TestCase(4, 0, 3)]
        [TestCase(5, 0, 2)]
        [TestCase(5, 0, 3)]
        [TestCase(5, 0, 4)]
        [TestCase(5, 1, 2)]
        [TestCase(5, 1, 3)]
        [TestCase(5, 1, 4)]
        [TestCase(5, 3, 2)]
        [TestCase(5, 3, 3)]
        [TestCase(5, 3, 4)]
        [TestCase(5, 4, 2)]
        [TestCase(5, 4, 3)]
        [TestCase(5, 4, 4)]
        public void Score(int n, int start, int len)
        {
            var mutation = CreateMutation(n, start, len);
            var mutationResult = mutation.GetResult();
            var expectedScore = new TspSolution(mutationResult.Order, mutationResult.Checkpoints).Score;
            Assert.AreEqual(expectedScore, mutation.Score, 1e-10);
        }

        private static FlipSegmentMutation CreateMutation(int n, int start, int len)
        {
            return new FlipSegmentMutation(
                new TspSolution(
                    Enumerable.Range(0, n).ToArray(),
                    checkpoints.Take(n).ToArray()),
                start,
                len);
        }
    }
}