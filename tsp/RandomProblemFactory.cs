using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AiAlgorithms.Tsp
{
    public class RandomProblemFactory
    {
        private readonly Random random;

        public RandomProblemFactory(Random random)
        {
            this.random = random;
        }

        public IEnumerable<Point[]> CreateProblems(int size, int count)
        {
            return Enumerable.Range(0, count).Select(i => CreateProblem(size));
        }

        public Point[] CreateProblem(int size)
        {
            var checkpoints = new Point[size];
            for (var j = 0; j < size; j++)
                checkpoints[j] = new Point(random.Next(10 * size), random.Next(10 * size));
            return checkpoints;
        }
    }
}