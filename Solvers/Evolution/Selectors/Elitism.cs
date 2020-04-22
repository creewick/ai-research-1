using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.Evolution.Selectors
{
    public class Elitism : IGeneticSelector
    {
        public IEnumerable<Solution> GetPopulation(State state, IEnumerable<Solution> parents, IEnumerable<Solution> children, int populationCount)
        {
            return parents
                .Take(1)
                .Concat(children
                     .Take(populationCount - 1)
                );
        }
    }
}