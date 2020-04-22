using System.Collections.Generic;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.Evolution.Appliers
{
    public interface IGeneticApplier
    {
        public IEnumerable<Solution> GetChildren(State state, List<Solution> solutions);
    }
}