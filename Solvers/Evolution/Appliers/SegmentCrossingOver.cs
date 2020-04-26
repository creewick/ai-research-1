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
                var firstParent = solutions[random.Next(solutions.Count)];
                var secondParent = solutions[random.Next(solutions.Count)];
                if (firstParent == secondParent) continue;
                var splitFirst = random.Next(firstParent.FirstCarCommandsList.Count());
                var splitSecond = random.Next(firstParent.SecondCarCommandsList.Count());
                
                yield return GetChild(firstParent, secondParent, splitFirst, splitSecond);
            }
        }
        
        private readonly Random random = new Random();
        
        private Solution GetChild(Solution firstParent, Solution secondParent, int splitFirst, int splitSecond)
        {
            return new Solution(
                CrossOver(firstParent.FirstCarCommandsList, secondParent.FirstCarCommandsList, splitFirst),
                CrossOver(firstParent.SecondCarCommandsList, secondParent.SecondCarCommandsList, splitSecond)
            );
        }

        private IEnumerable<Command> CrossOver(IEnumerable<Command> first, IEnumerable<Command> second, int split)
        {
            var firstPart = first.Take(split);
            var secondPart = second.Skip(split);

            return firstPart.Concat(secondPart);
        }
    }
}