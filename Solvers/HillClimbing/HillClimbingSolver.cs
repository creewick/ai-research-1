using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers
{
    public class HillClimbingSolver : ISolver
    {
        private readonly ISolver baseSolver = new GreedySolver();
        private readonly IMutator mutator = new FlipRandomSegmentMutator(10, 5);
        private readonly ISolver solver;

        private static double GetScore_3(State state)
        {
            var flags = new[]
                {
                    state.GetNextFlag(),
                    state.GetNextNextFlag()
                }
                .OrderBy(v => state.FirstCar.Pos.Dist2To(v));

            return 1000000 * state.FlagsTaken
                   - flags.First().Dist2To(state.FirstCar.Pos)
                   - flags.Last().Dist2To(state.SecondCar.Pos);
        }

        public HillClimbingSolver()
        {
            solver = new UniversalHillClimbingSolver(baseSolver, mutator, AggregateBy.Last, GetScore_3);
        }


        public IEnumerable<Solution> GetSolutions(State state, Countdown time) => solver.GetSolutions(state, time);
    }

    public class UniversalHillClimbingSolver : ISolver
    {
        protected bool ShouldContinue { get; set; }
        private readonly ISolver baseSolver;

        private readonly bool stopOnRepeatedMutation;
        private IMutation firstMutation;
        private int mutationsCount;
        private int improvementsCount;
        private IMutator mutator;
        private AggregateBy aggregate;
        private readonly Func<State, double> getScore;

        public UniversalHillClimbingSolver(ISolver baseSolver, IMutator mutator, AggregateBy aggregate,
            Func<State, double> getScore,
            bool stopOnRepeatedMutation = false)
        {
            this.baseSolver = baseSolver;
            this.mutator = mutator;
            this.aggregate = aggregate;
            this.getScore = getScore;
            this.stopOnRepeatedMutation = stopOnRepeatedMutation;
        }

        public IEnumerable<Solution> GetSolutions(State state, Countdown time)
        {
            mutationsCount = 0;
            improvementsCount = 0;
            ShouldContinue = true;
            var steps = new List<Solution>();
            steps.Add(baseSolver.GetSolutions(state, time / 10).Last());
            while (!time.IsFinished())
            {
                var improvements = Improve(state, steps.Last());
                mutationsCount++;
                foreach (var solution in improvements)
                {
                    improvementsCount++;
//                    if (solution is IHaveTime withTime) withTime.Time = time.TimeElapsed;
//                    if (solution is IHaveIndex withIndex)
//                    {
//                        withIndex.MutationIndex = mutationsCount;
//                        withIndex.ImprovementIndex = improvementsCount;
//                    }
                    steps.Add(solution);
                }

                if (!ShouldContinue) break;
                if (mutationsCount == 10)
                    break;
            }

            Console.WriteLine($"mutations: {mutationsCount}, improvements: {improvementsCount}");

            return steps;
        }

        protected IEnumerable<Solution> Improve(State state, Solution bestSolution)
        {
            //var improved = false;

            var mutation = mutator.Mutate(state, bestSolution);
            if (firstMutation == null)
                firstMutation = mutation;
            else if (stopOnRepeatedMutation && mutation.Equals(firstMutation))
                ShouldContinue = false;
            //if (mutation.Score > bestSolution.Score)
            if (Emulate(state, mutation.GetResult()) > Emulate(state, bestSolution))
            {
                bestSolution = mutation.GetResult();
                firstMutation = null;
                yield return bestSolution;
                //improved = true;
            }

            //ShouldContinue = improved;
        }

        private double Emulate(State state, Solution solution)
        {
            var copy = state.Copy();
            var stepsLeft = solution.FirstCarCommandsList.Count();
            var score = double.MinValue;
            var curSolution = solution;

            while (stepsLeft > 0)
            {
                copy.Tick(curSolution);
                curSolution = solution.GetNextTick();
                var newScore = getScore(copy);

                if (aggregate == AggregateBy.Last)
                    score = newScore;
                else if (aggregate == AggregateBy.Max && newScore > score)
                    score = newScore;

                stepsLeft--;
            }

            return score;
        }
    }
}