using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;
using AiAlgorithms.Algorithms;

namespace AI_Research_1.Solvers.Evolution.BaseSolvers
{
    public class CombinedSolver : ISolver
    {
        private readonly Dictionary<ISolver, double> solvers;
        private readonly int solutionsCount;

        public CombinedSolver(Dictionary<ISolver, double> solvers, int solutionsCount)
        {
            this.solvers = solvers;
            this.solutionsCount = solutionsCount;
        }

        public string GetNameWithArgs() => $"{solvers.Select(solver => solver.GetType().Name).StrJoin("+")}";

        public IEnumerable<Solution> GetSolutions(State state, Countdown time)
        {
            var ms = time.TimeAvailable;
            return solvers
                .SelectMany(solver => solver.Key
                    .GetSolutions(state, new Countdown(ms * solver.Value))
                    .TakeLast(solutionsCount / solvers.Count));
        }
    }
}