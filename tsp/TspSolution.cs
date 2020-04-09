using System;
using System.Drawing;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.Tsp
{
    public class TspSolution : IHaveTime, ISolution, IHaveIndex
    {
        public readonly Point[] Checkpoints;

        public int[] Order;

        public TspSolution(int[] order, Point[] checkpoints)
            : this(order, -checkpoints.GetPathLength(order), checkpoints)
        {
        }

        public TspSolution(int[] order, double score, Point[] checkpoints)
        {
            Order = order;
            Checkpoints = checkpoints;
            Score = score;
        }

        public TimeSpan Time { get; set; }
        public double Score { get; }
        public int MutationIndex { get; set; }
        public int ImprovementIndex { get; set; }
    }
}