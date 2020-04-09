using System;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.Tsp
{
    public class TransposeMutation : IMutation<TspSolution>
    {
        private readonly int i1;
        private readonly int i2;
        private readonly TspSolution parentSolution;

        public TransposeMutation(TspSolution parentSolution, in int i1, in int i2)
        {
            this.parentSolution = parentSolution;
            this.i1 = i1;
            this.i2 = i2;
            Score = parentSolution.Score - ScoreDiff();
        }

        public double Score { get; }

        public TspSolution GetResult()
        {
            var newOrder = Transpose(parentSolution.Order, i1, i2);
            var solution = new TspSolution(newOrder, parentSolution.Checkpoints);
            if (Math.Abs(solution.Score - Score) > 1e-5)
                throw new Exception($"{solution.Score} != {Score}");
            return new TspSolution(newOrder, Score, parentSolution.Checkpoints);
        }

        public static int[] Transpose(int[] order, int i1, int i2)
        {
            var copy = order.ToArray();
            var t = copy[i1];
            copy[i1] = copy[i2];
            copy[i2] = t;
            return copy;
        }

        public double ScoreDiff()
        {
            var order = parentSolution.Order;
            var size = parentSolution.Order.Length;
            var i = i1;
            var j = i2;

            int Ord(int index)
            {
                return order[(index + size) % size];
            }

            double D(int a, int b)
            {
                return Dist(Ord(a), Ord(b));
            }

            var diff = 0.0;
            diff -= D(i - 1, i);
            diff -= D(i, i + 1);
            diff -= D(j - 1, j);
            diff -= D(j, j + 1);
            diff += D(i - 1, j);
            diff += D(j, i + 1);
            diff += D(j - 1, i);
            diff += D(i, j + 1);
            var d = Math.Min((i - j + size) % size, (j - i + size) % size);
            if (d == 1)
                diff += 2 * D(i, j);
            return diff;
        }

        private double Dist(int iCheckpoint, int jCheckpoint)
        {
            var checkpoints = parentSolution.Checkpoints;
            return checkpoints[iCheckpoint].DistanceTo(checkpoints[jCheckpoint]);
        }
    }
}