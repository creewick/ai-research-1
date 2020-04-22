using System.Collections.Generic;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;
using AI_Research_1.Solvers.HillClimbing;

namespace AI_Research_1.Solvers.Evolution.Appliers
{
    public class MutationApplier : IGeneticApplier
    {
        private readonly IMutator mutator;

        public MutationApplier(IMutator mutator)
        {
            this.mutator = mutator;
        }

        public IEnumerable<Solution> GetChildren(State state, List<Solution> solutions)
        {
            while (true)
                foreach (var solution in solutions)
                    yield return mutator.Mutate(state, solution).GetResult();
        }
    }
}