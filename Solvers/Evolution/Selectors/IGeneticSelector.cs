using System.Collections.Generic;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.Evolution.Selectors
{
    public interface IGeneticSelector
    {
        public IEnumerable<Solution> GetPopulation(State state, IEnumerable<Solution> parents, IEnumerable<Solution> children, int populationCount);
    }
}