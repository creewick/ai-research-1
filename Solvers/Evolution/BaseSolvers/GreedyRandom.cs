using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.Evolution.BaseSolvers
{
    public class GreedyRandom : ISolver
    {
        private readonly int populationCount;
        private readonly int solutionDepth;
        private readonly RandomSearch randomSearch;
        private readonly GreedySolver greedySolver;

        public GreedyRandom(int populationCount, int solutionDepth)
        {
            this.populationCount = populationCount;
            this.solutionDepth = solutionDepth;
            this.randomSearch = new RandomSearch(populationCount, solutionDepth);
            this.greedySolver = new GreedySolver(solutionDepth);
        }
        
        public IEnumerable<Solution> GetSolutions(State state, Countdown time)
        {
            return greedySolver
                .GetSolutions(state, time / 2)
                .Take(populationCount / 2)
                .Concat(randomSearch
                    .GetSolutions(state, time / 2)
                    .Take(populationCount / 2));
        }
    }
}