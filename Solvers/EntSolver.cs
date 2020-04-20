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
    public class EntSolver : ISolver
    {
        private readonly ISolver solver;

        public EntSolver() =>
            solver = new UniversalRandomMegaBrainSolver(40, 8, AggregateBy.Max, 4, 8);

        public IEnumerable<Solution> GetSolutions(State state, Countdown time) => solver.GetSolutions(state, time);
        public string GetNameWithArgs() => solver.GetNameWithArgs();
    }

    public class UniversalRandomMegaBrainSolver : ISolver
    {
        private readonly int steps;
        private readonly int rndSegMaxLen;
        private readonly AggregateBy aggregate;
        private readonly Func<State, long> getScore;
        private List<(Solution, double)> solutions = new List<(Solution, double)>();
        private static Solution LastBestSolution;
        private readonly int thinkTicksCount;
        private readonly int movesCount;
        private int thinkIterations = 1;
        public bool IsMoving = false;

        public UniversalRandomMegaBrainSolver(int steps, int rndSegMaxLen, AggregateBy aggregate,
            int thinkTicksCount, int movesCount)
        {
            this.steps = steps;
            this.rndSegMaxLen = rndSegMaxLen;
            this.aggregate = aggregate;
            this.thinkTicksCount = thinkTicksCount;
            this.movesCount = movesCount;
        }

        public IEnumerable<Solution> GetSolutions(State state, Countdown time)
        {
            solutions = solutions.Select(x => (x.Item1.GetNextTick(), x.Item2)).ToList();
            if (IsMoving)
            {
                thinkIterations++;
                if (thinkIterations == movesCount)
                {
                    thinkIterations = 1;
                    IsMoving = false;
                    solutions = new List<(Solution, double)>();
                }
                else
                    return solutions.Select(x => x.Item1).ToList();
            }

            var random = new Random();
            

            while (!time.IsFinished())
            {
                var solution = new Solution(RandomCarSolution(random), RandomCarSolution(random));
                var score = Emulator.Emulate(state, solution, steps, aggregate);
                solutions.Add((solution, score));
            }

            var orderingSolutions = solutions.OrderBy(x => x.Item2).ToList();
            

            if (thinkIterations == thinkTicksCount)
            {
                thinkIterations = 1;
                IsMoving = true;
                solutions = new List<(Solution, double)> {orderingSolutions.Last()};
                return solutions.Select(x => x.Item1).ToList();
            }
            else
            {
                thinkIterations++;
            }

            return orderingSolutions.Select(x => x.Item1).ToList();
        }

        public string GetNameWithArgs() =>
            $"Random.{steps}.{rndSegMaxLen}.{aggregate}.{getScore.Method.Name}";

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
            var thinkSteps = thinkTicksCount - thinkIterations;
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

            for (var i = 0; i < thinkSteps; i++)
            {
                carSolution[i] = new Move(0, 0);
            }

            return carSolution;
        }
    }
}