using System.Collections.Generic;

namespace AiAlgorithms.Algorithms
{
    public abstract class RandomSolver<TProblem, TSolution> : ISolver<TProblem, TSolution> where TSolution : ISolution
    {
        private readonly bool returnAllSolutions;

        protected RandomSolver(bool returnAllSolutions = false)
        {
            this.returnAllSolutions = returnAllSolutions;
        }

        public abstract TSolution GenerateRandomSolution(TProblem problem);

        public IEnumerable<TSolution> GetSolutions(TProblem problem, Countdown countdown)
        {
            yield return GenerateRandomSolution(problem);
            var simCount = 0;
            var improvementsCount = 0;
            var bestScore = double.NegativeInfinity;
            while (!countdown.IsFinished())
            {
                var solution = GenerateRandomSolution(problem);
                simCount++;
                var scoreIsBetter = solution.Score > bestScore;
                if (scoreIsBetter)
                {
                    improvementsCount++;
                    bestScore = solution.Score;
                }
                if (returnAllSolutions || scoreIsBetter)
                {
                    if (solution is IHaveTime withTime) withTime.Time = countdown.TimeElapsed;
                    if (solution is IHaveIndex withIndex)
                    {
                        withIndex.MutationIndex = simCount;
                        withIndex.ImprovementIndex = improvementsCount;
                    }

                    yield return solution;
                }
            }
        }
    }
}