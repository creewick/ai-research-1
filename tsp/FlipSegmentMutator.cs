using System.Drawing;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.Tsp
{
    public class FlipSegmentMutator : IMutator<Point[], TspSolution>
    {
        private int len = 2;
        private int start;

        public IMutation<TspSolution> Mutate(Point[] problem, TspSolution parentSolution)
        {
            var size = parentSolution.Order.Length;
            var mutation = new FlipSegmentMutation(parentSolution, start, len);
            start++;
            if (start >= size)
            {
                start = 0;
                len++;
                if (len >= size / 2)
                    len = 2;
            }

            return mutation;
        }
    }
}