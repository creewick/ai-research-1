using System.Collections.Generic;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.Evolution.Filters
{
    public interface IGeneticFilter
    {
        public List<Solution> GetParents(State state, List<Solution> solutions);
    }
}