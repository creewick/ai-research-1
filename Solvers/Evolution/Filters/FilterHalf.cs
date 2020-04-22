using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.Evolution.Filters
{
    public class FilterHalf : IGeneticFilter
    {
        public List<Solution> GetParents(State state, List<Solution> solutions)
        {
            return Emulator
                .SortByScore(solutions, state)
                .Take(solutions.Count / 2)
                .ToList();
        }
    }
}