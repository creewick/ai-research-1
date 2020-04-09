using System;
using System.Drawing;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.Tsp
{
    public class TransposeMutator : IMutator<Point[], TspSolution>
    {
        private readonly Random random;

        public TransposeMutator(Random random)
        {
            this.random = random;
        }

        public IMutation<TspSolution> Mutate(Point[] problem, TspSolution parentSolution)
        {
            var n = parentSolution.Order.Length;
            var i = random.Next(n);
            var j = random.Next(n);
            while (j == i)
                j = random.Next(n);
            return new TransposeMutation(parentSolution, i, j);
        }

        public override string ToString()
        {
            return "Transpose";
        }
    }
}