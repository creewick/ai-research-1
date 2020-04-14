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
        private readonly ISolver baseSolver =
            new UniversalGreedySolver(17, SimulateBy.Repeat, AggregateBy.Max, Emulator.GetScore_1);

        private readonly List<IMutator> mutators = new List<IMutator>()
        {
            new RandomSegmentMutator(3, 1),
            new FlipRandomSegmentMutator(3,3),
            new SwapTwoRandomSegmentsMutator(3)
        };

        private readonly ISolver solver;


        public HillClimbingSolver()
        {
            solver = new UniversalHillClimbingSolver(baseSolver, mutators, AggregateBy.Max, Emulator.GetScore_3);
        }

        public IEnumerable<Solution> GetSolutions(State state, Countdown time) => solver.GetSolutions(state, time);
    }

    public class UniversalHillClimbingSolver : ISolver
    {
        protected bool ShouldContinue { get; set; }
        private readonly ISolver baseSolver;

        private readonly bool useBestSolution;
        private IMutation firstMutation;
        public int MutationsCount;

        public int ImprovementsCount;
        private AggregateBy aggregate;
        private readonly Func<State, long> getScore;
        private Solution bestSolution;
        public int BestSolutionsWinsCount = 0;
        private List<IMutator> mutators;


        public UniversalHillClimbingSolver(ISolver baseSolver, List<IMutator> mutators, AggregateBy aggregate,
            Func<State, long> getScore,
            bool useBestSolution = true)
        {
            this.baseSolver = baseSolver;
            this.mutators = mutators;
            this.aggregate = aggregate;
            this.getScore = getScore;
            this.useBestSolution = useBestSolution;
        }

        public IEnumerable<Solution> GetSolutions(State state, Countdown time)
        {
            MutationsCount = 0;
            ImprovementsCount = 0;
            ShouldContinue = true;
            var steps = new List<Solution>();

            var baseSolverSolution = baseSolver.GetSolutions(state, time / 10).Last();
            steps.Add(baseSolverSolution);

            if (bestSolution != null && useBestSolution)
                UpdateBestSolution(state);
            if (bestSolution == null)
                bestSolution = baseSolverSolution;
            while (!time.IsFinished())
            {
                var improvements = Improve(state, steps.Last());
                MutationsCount += mutators.Count;
                foreach (var solution in improvements)
                {
                    ImprovementsCount++;
                    steps.Add(solution);
                }

                if (!ShouldContinue) break;
            }

            if (useBestSolution)
            {
                var currentBestScore = Emulate(state, steps.Last());
                var previousBestScore = Emulate(state, bestSolution);
                if (currentBestScore > previousBestScore)
                {
                    steps.Insert(0, bestSolution);
                    bestSolution = steps.Last();
                }
                else
                {
                    steps.Add(bestSolution);
                    BestSolutionsWinsCount++;
                    Console.WriteLine("best");
                }
            }

            foreach (var step in steps)
            {
                Console.Write($"{Emulate(state, step)} ");
            }

            Console.WriteLine($"mutations: {MutationsCount}, improvements: {ImprovementsCount}");
            return steps;
        }

        private void UpdateBestSolution(State state)
        {
            var solution = bestSolution.GetNextTick();
            var firstCarCommands = solution.FirstCarCommandsList.ToList();
            var secondCarCommands = solution.SecondCarCommandsList.ToList();
            firstCarCommands.Add(null);
            secondCarCommands.Add(null);
            var newCommandIndex = firstCarCommands.Count - 1;
            var bestScore = long.MinValue;
            Solution newBestSolution = null;
            foreach (var command in Command.All)
            {
                foreach (var anotherCommand in Command.All)
                {
                    firstCarCommands[newCommandIndex] = command;
                    secondCarCommands[newCommandIndex] = anotherCommand;
                    var currentSolution = new Solution(firstCarCommands, secondCarCommands);
                    var score = Emulate(state, currentSolution);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        newBestSolution = currentSolution;
                    }
                }
            }

            bestSolution = newBestSolution;
        }

        protected IEnumerable<Solution> Improve(State state, Solution improvingSolution)
        {
            var bestScore = long.MinValue;
            IMutation bestMutation = null;
            foreach (var mutator in mutators)
            {
                var mutation = mutator.Mutate(state, improvingSolution);
                var mutationSolution = mutation.GetResult();
                var score = Emulate(state, mutationSolution);
                if (score > bestScore)
                {
                    bestMutation = mutation;
                    bestScore = score;
                }
            }

            if (bestScore > Emulate(state, improvingSolution))
            {
                yield return bestMutation.GetResult();
                Console.WriteLine(bestMutation.GetType());
            }
        }

        private long Emulate(State state, Solution solution) =>
            Emulator.Emulate(state, solution, solution.FirstCarCommandsList.Count(), aggregate, getScore);
    }
}