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
            return parents
                .Concat(children)
                .OrderByDescending(s =>
                    Emulator.Emulate(state, s, s.FirstCarCommandsList.Count()))
                .Take(populationCount);
        }
    }
}