using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Solvers.HillClimbing.Mutators;

namespace AI_Research_1.Solvers.Evolution.Appliers
{
    public class RandomNoiseSegment : IGeneticApplier
    {
        private readonly RandomNoiseSegmentMutator mutator = new RandomNoiseSegmentMutator();
        
        public IEnumerable<Solution> GetChildren(List<Solution> solutions)
        {
            return solutions
                .Select(s => mutator.Mutate(null, s).GetResult());
        }
    }
}