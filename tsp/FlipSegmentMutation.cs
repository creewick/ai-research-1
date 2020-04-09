using System;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.Tsp
{
    public class FlipSegmentMutation : IMutation<TspSolution>
    {
        private readonly int len;
        private readonly TspSolution parentSolution;
        private readonly int start;

        public FlipSegmentMutation(TspSolution parentSolution, in int start, in int len)
        {
            this.parentSolution = parentSolution;
            this.start = start;
            this.len = len;
            Score = parentSolution.Score - ScoreDiff();
        }

        public double Score { get; }

        public TspSolution GetResult()
        {
            var order = parentSolution.Order;
            var newOrder = order.ToArray();
            for (var i = 0; i < len; i++)
                newOrder[(start + i) % order.Length] = order[(start + len - i - 1) % order.Length];
            var solution = new TspSolution(newOrder, parentSolution.Checkpoints);
            if (Math.Abs(solution.Score - Score) > 1e-5)
                throw new Exception($"{solution.Score} != {Score}");
            return solution;
        }

        private double ScoreDiff()
        {
            var order = parentSolution.Order;
            var size = order.Length;
            var prev = order[(start - 1 + size) % size];
            var first = order[start];
            var last = order[(start + len - 1) % size];
            var next = order[(start + len) % size];
            return
                -Dist(prev, first) - Dist(last, next)
                + Dist(prev, last) + Dist(first, next);
        }

        private double Dist(int iCheckpoint, int jCheckpoint)
        {
            var checkpoints = parentSolution.Checkpoints;
            return checkpoints[iCheckpoint].DistanceTo(checkpoints[jCheckpoint]);
        }

        protected bool Equals(FlipSegmentMutation other)
        {
            return len == other.len && start == other.start;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FlipSegmentMutation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (len * 397) ^ start;
            }
        }

        public static bool operator ==(FlipSegmentMutation left, FlipSegmentMutation right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FlipSegmentMutation left, FlipSegmentMutation right)
        {
            return !Equals(left, right);
        }
    }
}