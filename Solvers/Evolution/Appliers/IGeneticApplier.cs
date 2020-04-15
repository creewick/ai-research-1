using System.Collections.Generic;
using AI_Research_1.Interfaces;

namespace AI_Research_1.Solvers.Evolution.Appliers
{
    public interface IGeneticApplier
    {
        public IEnumerable<Solution> GetChildren(List<Solution> solutions);
    }
}