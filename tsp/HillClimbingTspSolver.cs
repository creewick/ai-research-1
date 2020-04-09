using System.Collections.Generic;
using System.Drawing;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.Tsp
{
    public class HillClimbingTspSolver : AbstractImprover<Point[], TspSolution>
    {
        public HillClimbingTspSolver(ISolver<Point[], TspSolution> baseSolver)
            : base(baseSolver)
        {
        }

        protected override IEnumerable<TspSolution> Improve(Point[] problem, TspSolution bestSolution)
        {
            var improved = false;
            var checkpoints = bestSolution.Checkpoints;
            var n = checkpoints.Length;
            for (var i = 0; i < n - 1; i++)
            for (var j = i + 1; j < n; j++)
            {
                var mutation = new TransposeMutation(bestSolution, i, j);
                if (mutation.Score > bestSolution.Score)
                {
                    bestSolution = mutation.GetResult();
                    yield return bestSolution;
                    improved = true;
                }
            }

            ShouldContinue = improved;
        }
    }
}