using System;
using System.Drawing;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.Tsp
{
    public class TransposeNeighborsMutator : IMutator<Point[], TspSolution>
    {
        private readonly Random random;

        public TransposeNeighborsMutator(Random random)
        {
            this.random = random;
        }

        public IMutation<TspSolution> Mutate(Point[] problem, TspSolution parentSolution)
        {
            var n = parentSolution.Order.Length;
            var i = random.Next(n - 1);
            return new TransposeMutation(parentSolution, i, i + 1);
        }

        public override string ToString()
        {
            return "TransposeNeighbors";
        }
    }
}