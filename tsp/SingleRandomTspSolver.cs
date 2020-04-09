using System;
using System.Collections.Generic;
using System.Drawing;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.Tsp
{
    public class SingleRandomTspSolver : ISolver<Point[], TspSolution>
    {
        private readonly Random random;

        public SingleRandomTspSolver(Random random)
        {
            this.random = random;
        }

        public IEnumerable<TspSolution> GetSolutions(Point[] checkpoints, Countdown countdown)
        {
            return new List<TspSolution> {RandomTspSolver.GetRandomOrder(checkpoints, random)};
        }
    }
}