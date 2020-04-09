using System;
using System.Drawing;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.Tsp
{
    public class RandomTspSolver : RandomSolver<Point[], TspSolution>
    {
        private readonly Random random;

        public RandomTspSolver(Random random, bool returnAllSolutions = false)
            : base(returnAllSolutions)
        {
            this.random = random;
        }

        public override TspSolution GenerateRandomSolution(Point[] checkpoints)
        {
            return GetRandomOrder(checkpoints, random);
        }

        public static TspSolution GetRandomOrder(Point[] checkpoints, Random random)
        {
            var order = Enumerable.Range(0, checkpoints.Length).ToArray();
            for (var i = 0; i < order.Length; i++)
            {
                var index = random.Next(order.Length - i);
                var t = order[index];
                order[index] = order[order.Length - i - 1];
                order[order.Length - i - 1] = t;
            }

            return new TspSolution(order, checkpoints);
        }
    }
}