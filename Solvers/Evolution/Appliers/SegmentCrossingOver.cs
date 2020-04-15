using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;

namespace AI_Research_1.Solvers.Evolution.Appliers
{
    public class SegmentCrossingOver : IGeneticApplier
    {
        private static readonly Random Random = new Random();
        
        public IEnumerable<Solution> GetChildren(List<Solution> solutions)
        {
            for (var i = 0; i < solutions.Count; i++)
            for (var j = i + 1; j < solutions.Count; j++)
                yield return GetChild(solutions[i], solutions[j]);
        }

        private static Solution GetChild(Solution firstParent, Solution secondParent)
        {
            var splitFirstCar = Random.Next(firstParent.FirstCarCommandsList.Count());
            var splitSecondCar = Random.Next(firstParent.SecondCarCommandsList.Count());
            
            return new Solution(
                CrossOver(firstParent.FirstCarCommandsList, secondParent.FirstCarCommandsList, splitFirstCar),
                CrossOver(firstParent.SecondCarCommandsList, secondParent.SecondCarCommandsList, splitSecondCar)
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