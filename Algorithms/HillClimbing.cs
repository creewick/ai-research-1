using System.Collections.Generic;
using System.Linq;

namespace AiAlgorithms.Algorithms
{
    public class HillClimbing<TProblem, TSolution> : ISolver<TProblem, TSolution>
        where TSolution : ISolution
    {
        private readonly ISolver<TProblem, TSolution> baseSolver;
        protected readonly IMutator<TProblem, TSolution> mutator;
        private readonly bool stopOnRepeatedMutation;
        private IMutation<TSolution> firstMutation;
        private int mutationsCount;
        private int improvementsCount;

        public HillClimbing(ISolver<TProblem, TSolution> baseSolver, IMutator<TProblem, TSolution> mutator,
            bool stopOnRepeatedMutation = false)
        {
            this.baseSolver = baseSolver;
            this.mutator = mutator;
            this.stopOnRepeatedMutation = stopOnRepeatedMutation;
        }

        protected bool ShouldContinue { get; set; }

        public IEnumerable<TSolution> GetSolutions(TProblem problem, Countdown countdown)
        {
            mutationsCount = 0;
            improvementsCount = 0;
            ShouldContinue = true;
            var steps = new List<TSolution>();
            steps.Add(baseSolver.GetSolutions(problem, countdown / 2).Last());
            while (!countdown.IsFinished())
            {
                var improvements = Improve(problem, steps.Last());
                mutationsCount++;
                foreach (var solution in improvements)
                {
                    improvementsCount++;
                    if (solution is IHaveTime withTime) withTime.Time = countdown.TimeElapsed;
                    if (solution is IHaveIndex withIndex)
                    {
                        withIndex.MutationIndex = mutationsCount;
                        withIndex.ImprovementIndex = improvementsCount;
                    }
                    steps.Add(solution);
                }

                if (!ShouldContinue) break;
            }

            return steps;
        }

        protected IEnumerable<TSolution> Improve(TProblem problem, TSolution bestSolution)
        {
            var mutation = mutator.Mutate(problem, bestSolution);
            if (firstMutation == null)
                firstMutation = mutation;
            else if (stopOnRepeatedMutation && mutation.Equals(firstMutation))
                ShouldContinue = false;
            if (mutation.Score > bestSolution.Score)
            {
                bestSolution = mutation.GetResult();
                firstMutation = null;
                yield return bestSolution;
            }
        }
    }
}