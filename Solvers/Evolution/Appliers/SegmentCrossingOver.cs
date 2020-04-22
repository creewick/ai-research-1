using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.Evolution.Appliers
{
    public class SegmentCrossingOver : IGeneticApplier
    {
        public IEnumerable<Solution> GetChildren(State state, List<Solution> solutions)
        {
            while (true)
            {
                var firstParent = solutions[Random.Next(solutions.Count)];
                var secondParent = solutions[Random.Next(solutions.Count)];
                if (firstParent == secondParent) continue;
                var splitFirst = Random.Next(firstParent.FirstCarCommandsList.Count());
                var splitSecond = Random.Next(firstParent.SecondCarCommandsList.Count());
                
                yield return GetChild(firstParent, secondParent, splitFirst, splitSecond);
            }
        }
        private static readonly Random Random = new Random();
        
        private static Solution GetChild(Solution firstParent, Solution secondParent, int splitFirst, int splitSecond)
        {
            return new Solution(
                CrossOver(firstParent.FirstCarCommandsList, secondParent.FirstCarCommandsList, splitFirst),
                CrossOver(firstParent.SecondCarCommandsList, secondParent.SecondCarCommandsList, splitSecond)
            );
        }

        private static IEnumerable<Command> CrossOver(IEnumerable<Command> first, IEnumerable<Command> second, int split)
        {
            var firstPart = first.Take(split);
            var secondPart = second.Skip(split);

            return firstPart.Concat(secondPart);
        }

    }
}