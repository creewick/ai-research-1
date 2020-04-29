using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Interfaces.Commands;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers
{
    public class GreedySolver : ISolver
    {
        private readonly ISolver solver;

        public GreedySolver(int solutionDepth, Func<State, long> getScore=null)
        {
            getScore ??= Emulator.GetScore;
            solver = new UniversalGreedySolver(solutionDepth, SimulateBy.Repeat, AggregateBy.Max, getScore);
        }

        public string GetNameWithArgs() => solver.GetNameWithArgs();

        public IEnumerable<Solution> GetSolutions(State state, Countdown time) => solver.GetSolutions(state, time);
    }

    public class UniversalGreedySolver : ISolver
    {
        private readonly int steps;
        private readonly SimulateBy simulate;
        private readonly AggregateBy aggregate;
        private readonly Func<State, long> getScore;

        public UniversalGreedySolver(int steps, SimulateBy simulate, AggregateBy aggregate, Func<State, long> getScore)
        {
            this.steps = steps;
            this.simulate = simulate;
            this.aggregate = aggregate;
            this.getScore = getScore;
        }

        public IEnumerable<Solution> GetSolutions(State state, Countdown time)
        {
            var solutions = new List<(Solution, long)>();

            foreach (var firstCarMove in Command.All)
            foreach (var secondCarMove in Command.All)
            {
                var solution = simulate == SimulateBy.Repeat
                    ? new Solution(
                        Enumerable.Repeat(firstCarMove, steps),
                        Enumerable.Repeat(secondCarMove, steps)
                    )
                    : new Solution(
                        Enumerable.Repeat(new Move(0,0), steps-1).Prepend(firstCarMove),
                        Enumerable.Repeat(new Move(0,0), steps-1).Prepend(secondCarMove)
                    );
                
                var score = Emulator.Emulate(state, solution, steps, aggregate);
                solutions.Add((solution, score));
                
                if (time.IsFinished())
                    return solutions.OrderBy(x => x.Item2).Select(x => x.Item1); 
            }

            return solutions.OrderBy(x => x.Item2).Select(x => x.Item1);
        }

        public string GetNameWithArgs() => $"Greedy.{steps}.{simulate}.{aggregate}";
    }

    public enum SimulateBy { DoNothing, Repeat }
}