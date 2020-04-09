using System.Collections.Generic;
using System.Linq;

namespace AiAlgorithms.Algorithms
{
    public abstract class AbstractImprover<TProblem, TSolution> : ISolver<TProblem, TSolution>
        where TSolution : ISolution
    {
        private readonly ISolver<TProblem, TSolution> baseSolver;

        protected AbstractImprover(ISolver<TProblem, TSolution> baseSolver)
        {
            this.baseSolver = baseSolver;
        }

        protected bool ShouldContinue { get; set; }

        public IEnumerable<TSolution> GetSolutions(TProblem problem, Countdown countdown)
        {
            ShouldContinue = true;
            var steps = baseSolver.GetSolutions(problem, countdown / 10).ToList();
            while (!countdown.IsFinished())
            {
                var improvements = Improve(problem, steps.Last());
                foreach (var solution in improvements)
                {
                    if (solution is IHaveTime withTime) withTime.Time = countdown.TimeElapsed;
                    steps.Add(solution);
                }

                if (!ShouldContinue) break;
            }

            return steps;
        }

        protected abstract IEnumerable<TSolution> Improve(TProblem problem, TSolution bestSolution);
    }
}