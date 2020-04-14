using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;
using AiAlgorithms.Algorithms;

namespace AI_Research_1.Solvers
{
    public class RandomSolver : ISolver
    {
        public IEnumerable<Solution> GetSolutions(State state, Countdown time) =>
            new UniversalRandomSolver(10, 10, AggregateBy.Max, Emulator.DefaultGetScore).GetSolutions(state, time);
    }

    public class UniversalRandomSolver : ISolver
    {
        private readonly int steps;
        private readonly int rndSegMaxLen;
        private readonly AggregateBy aggregate;
        private readonly Func<State, long> getScore;

        public UniversalRandomSolver(int steps, int rndSegMaxLen, AggregateBy aggregate, Func<State, long> getScore)
        {
            this.steps = steps;
            this.rndSegMaxLen = rndSegMaxLen;
            this.aggregate = aggregate;
            this.getScore = getScore;
        }

        public IEnumerable<Solution> GetSolutions(State state, Countdown time)
        {
            var random = new Random();
            var solutions = new List<(Solution, double)>();

            while (!time.IsFinished())
            {
                var solution = new Solution(CarSolution(random), CarSolution(random));
                var score = Emulator.Emulate(state, solution, steps, aggregate, getScore);
                solutions.Add((solution, score));
            }

            return solutions
                .OrderBy(x => x.Item2)
                .Select(x => x.Item1);
        }

        private List<Command> CarSolution(Random random)
        {
            var carSolution = new List<Command>();
            while (carSolution.Count < steps)
            {
                var count = random.Next(Math.Min(rndSegMaxLen, steps - carSolution.Count));
                var command = random.Choice(Command.All.ToList());
                carSolution.AddRange(Enumerable.Repeat(command, count + 1));
            }

            return carSolution;
        }
    }
}