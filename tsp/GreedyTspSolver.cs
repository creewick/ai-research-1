using System;
using System.Collections.Generic;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.Tsp
{
    public class GreedyTspSolver : ISolver<Point[], TspSolution>
    {
        public IEnumerable<TspSolution> GetSolutions(Point[] checkpoints, Countdown countdown)
        {
            return new List<TspSolution> { SolveWithStart(checkpoints, 0) };

            //return 
            //    Enumerable.Range(0, checkpoints.Length)
            //        .Select(i => SolveWithStart(checkpoints, i))
            //        .OrderBy(s => s.Score)
            //    .ToList();
        }

        private static TspSolution SolveWithStart(Point[] checkpoints, int index)
        {
            var order = new List<int>();
            var used = new bool[checkpoints.Length];
            var lastPoint = checkpoints[index];
            order.Add(index);
            used[index] = true;
            for (var i = 1; i < checkpoints.Length; i++)
            {
                var closestUnusedPointIndex = -1;
                var bestDistance = double.PositiveInfinity;
                for (var iNext = 0; iNext < checkpoints.Length; iNext++)
                {
                    if (used[iNext]) continue;
                    var dist = checkpoints[iNext].DistanceTo(lastPoint);
                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        closestUnusedPointIndex = iNext;
                    }
                }

                used[closestUnusedPointIndex] = true;
                order.Add(closestUnusedPointIndex);
                lastPoint = checkpoints[closestUnusedPointIndex];
            }

            var orderArray = order.ToArray();
            return new TspSolution(orderArray, checkpoints);
        }
    }
}