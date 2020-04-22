using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.Evolution.Filters
{
    public class NormalizeFilter : IGeneticFilter
    {
        private readonly Random random = new Random();

        public List<Solution> GetParents(State state, List<Solution> solutions)
        {
            var scores = new Dictionary<Solution, long>();
            var maxScore = long.MinValue;

            foreach (var solution in solutions)
            {
                var score = Emulator.Emulate(state, solution, solution.FirstCarCommandsList.Count());
                scores[solution] = score;
                if (score > maxScore)
                    maxScore = score;
            }

            return solutions
                .Where(s => scores[s] / maxScore <= (long) random.NextDouble())
                .ToList();
        }
    }
}