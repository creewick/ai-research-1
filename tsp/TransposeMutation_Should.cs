using System.Drawing;
using NUnit.Framework;

namespace AiAlgorithms.Tsp
{
    [TestFixture]
    public class TransposeMutation_Should
    {
        [Test]
        public void CalculateScore([Values(0, 1, 2, 3)] int a, [Values(0, 1, 2, 3)] int b)
        {
            var mutation = new TransposeMutation(
                new TspSolution(new[] {0, 1, 2, 3},
                    new[] {new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(0, 1)}), a, b);
            var newSolution = mutation.GetResult();
            Assert.AreEqual(newSolution.Score, mutation.Score, 1e-10);
        }
    }
}