using System;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.Tsp
{
    public class FlipRandomSegmentMutator : IMutator<Point[], TspSolution>
    {
        private readonly Random random;

        public FlipRandomSegmentMutator(Random random)
        {
            this.random = random;
        }

        public IMutation<TspSolution> Mutate(Point[] problem, TspSolution parentSolution)
        {
            var size = parentSolution.Order.Length;
            var start = random.Next(size);
            var len = random.Next(2, size);
            return new FlipSegmentMutation(parentSolution, start, len);
        }
    }
}