using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;
using AI_Research_1.Solvers.HillClimbing.Mutators;
using AiAlgorithms.Algorithms;

namespace AI_Research_1.Solvers.HillClimbing
{
    public class HillClimbingSolver : ISolver
    {
        private readonly ISolver baseSolver = new RandomSolver();

        private readonly List<IMutator> mutators = new List<IMutator>()
        {
            new RandomNoiseSegmentMutator(),
            new RandomRepeatSegmentMutator(),
            new RandomAndDoNothingSegmentMutator(),
            new FlipRandomSegmentMutator(),
            new SwapTwoRandomSegmentsMutator(),
            new ExchangeAndSwapMutator()
        };

        private readonly ISolver solver;

        public HillClimbingSolver()
        {
            solver = new UniversalHillClimbingSolver(baseSolver, mutators, AggregateBy.Max);
        }

        public string GetNameWithArgs() => solver.GetNameWithArgs();

        public IEnumerable<Solution> GetSolutions(State state, Countdown time) => solver.GetSolutions(state, time);
    }

    public class UniversalHillClimbingSolver : ISolver
    {
        private readonly ISolver baseSolver;
        private readonly bool useBestSolution;
        private readonly AggregateBy aggregate;
        private Solution bestSolution;
        private readonly List<IMutator> mutators;
        public readonly UniversalHillClimbingSolverTelemetry Telemetry = new UniversalHillClimbingSolverTelemetry();

        public UniversalHillClimbingSolver(ISolver baseSolver, List<IMutator> mutators, AggregateBy aggregate,
            bool useBestSolution = true)
        {
            this.baseSolver = baseSolver;
            this.mutators = mutators;
            this.aggregate = aggregate;
            this.useBestSolution = useBestSolution;
        }

        public IEnumerable<Solution> GetSolutions(State state, Countdown time)
        {
            Telemetry.ImprovementsCount = 0;
            Telemetry.MutationsCount = 0;
            Telemetry.BestSolutionsWinsCount = 0;
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
                Telemetry.MutationsCount += mutators.Count;
                foreach (var solution in improvements)
                {
                    Telemetry.ImprovementsCount++;
                    steps.Add(solution);
                }
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
                    Telemetry.BestSolutionsWinsCount++;
                }
            }

            foreach (var step in steps)
            {
                Debug.Write($"{Emulate(state, step)} ");
            }

            Telemetry.TotalImprovementsCount += Telemetry.ImprovementsCount;
            Telemetry.TotalMutationsCount += Telemetry.MutationsCount;
            Telemetry.BestSolutionsWinsCount += Telemetry.BestSolutionsWinsCount;
            Debug.WriteLine(
                $"mutations: {Telemetry.MutationsCount}, improvements: {Telemetry.ImprovementsCount},use best: {Telemetry.BestSolutionsWinsCount}");
            return steps;
        }

        public string GetNameWithArgs() =>
            $"HillClimbing.{baseSolver.GetNameWithArgs()};{mutators.Select(m => m.GetType().Name).StrJoin(".")};{aggregate}.H={useBestSolution}";

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

        private IEnumerable<Solution> Improve(State state, Solution improvingSolution)
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
                Telemetry.AddMutationWin(bestMutation);
                Debug.WriteLine(bestMutation.GetType());
            }
        }

        private long Emulate(State state, Solution solution) =>
            Emulator.Emulate(state, solution, solution.FirstCarCommandsList.Count(), aggregate);
    }
}