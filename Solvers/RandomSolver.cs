using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Interfaces.Commands;
using AI_Research_1.Logic;
using AiAlgorithms.Algorithms;

namespace AI_Research_1.Solvers
{
    public class RandomSolver : ISolver
    {
        private readonly ISolver solver;
        public RandomSolver(bool withHeuristic = true) =>
            solver = new UniversalRandomSolver(10, 8, AggregateBy.Max, Emulator.DefaultGetScore, withHeuristic);
        public IEnumerable<Solution> GetSolutions(State state, Countdown time) => solver.GetSolutions(state, time);
    }

    public class UniversalRandomSolver : ISolver
    {
        private readonly int steps;
        private readonly int rndSegMaxLen;
        private readonly AggregateBy aggregate;
        private readonly Func<State, long> getScore;

        private static Solution LastBestSolution;
        private readonly bool useLastBastSolution;

        public UniversalRandomSolver(int steps, int rndSegMaxLen, AggregateBy aggregate, Func<State, long> getScore, bool useLastBastSolution)
        {
            this.steps = steps;
            this.rndSegMaxLen = rndSegMaxLen;
            this.aggregate = aggregate;
            this.getScore = getScore;
            this.useLastBastSolution = useLastBastSolution;
        }

        public IEnumerable<Solution> GetSolutions(State state, Countdown time)
        {
            var random = new Random();
            var solutions = new List<(Solution, double)>();
            if (useLastBastSolution && LastBestSolution != null)
            {
                foreach (var first in UpdateLastSolution(LastBestSolution.FirstCarCommandsList))
                foreach (var second in UpdateLastSolution(LastBestSolution.SecondCarCommandsList))
                {
                    var s = new Solution(first, second);
                    solutions.Add((s, Emulator.Emulate(state, s, steps, aggregate, getScore)));
                }
            }

            while (!time.IsFinished())
            {
                var solution = new Solution(RandomCarSolution(random), RandomCarSolution(random));
                var score = Emulator.Emulate(state, solution, steps, aggregate, getScore);
                solutions.Add((solution, score));
            }

            var orderingSolutions = solutions.OrderBy(x => x.Item2).ToList();
            if (useLastBastSolution) LastBestSolution = orderingSolutions.Last().Item1;

            return orderingSolutions.Select(x => x.Item1);
        }

        private static IEnumerable<List<Command>> UpdateLastSolution(IEnumerable<Command> lastSolution)
        {
            var baseSolution = lastSolution.Skip(1);
            foreach (var command in Command.All)
            {
                var newSolution = baseSolution.ToList();
                newSolution.Add(command);
                yield return newSolution;
            }
        }

        private List<Command> RandomCarSolution(Random random)
        {
            var carSolution = new List<Command>();
            while (carSolution.Count < steps)
            {
                var command = random.Choice(Command.All.ToList());
                if (command is Exchange)
                {
                    carSolution.Add(command);
                    continue;
                }
                var count = random.Next(Math.Min(rndSegMaxLen, steps - carSolution.Count));
                carSolution.AddRange(Enumerable.Repeat(command, count + 1));
            }

            return carSolution;
        }
    }
}