using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.Evolution.Selectors
{
    public class SelectTopScore : IGeneticSelector
    {
        public IEnumerable<Solution> GetPopulation(State state, List<Solution> parents, List<Solution> children, int populationCount)
        {
            return Emulator
                .SortByScore(parents, state)
                .Take(1)
                .Concat(Emulator
                    .SortByScore(children, state)
                    .Take(populationCount - 1)
                );
        }
    }
}