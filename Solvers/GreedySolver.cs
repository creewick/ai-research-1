using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers
{
    public class GreedySolver : ISolver
    {
        private static double GetScore_1(State state) => 
            + 1000000 * state.FlagsTaken
            - state.GetNextFlag().Dist2To(state.FirstCar.Pos)
            - state.GetNextFlag().Dist2To(state.SecondCar.Pos);

        private static double GetScore_2(State state)
        {
            var taken = state.FlagsTaken;
            var all = state.Track.Flags.Count;
            var firstCarFlag = state.Track.Flags[(taken + taken % 2) % all];
            var secondCarFlag = state.Track.Flags[(taken + 1 - taken % 2) % all];

            return 1000000 * state.FlagsTaken
               - firstCarFlag.Dist2To(state.FirstCar.Pos)
               - secondCarFlag.Dist2To(state.SecondCar.Pos);
        }

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
        
        private static readonly ISolver Solver =
            new UniversalGreedySolver(17, SimulateBy.Repeat, AggregateBy.Max, GetScore_3);

        public IEnumerable<Solution> GetSolutions(State state, Countdown time) => Solver.GetSolutions(state, time);
    }

    public class UniversalGreedySolver : ISolver
    {
        private readonly int steps;
        private readonly SimulateBy simulate;
        private readonly AggregateBy aggregate;
        private readonly Func<State, double> getScore;

        public UniversalGreedySolver(int steps, SimulateBy simulate, AggregateBy aggregate, Func<State, double> getScore)
        {
            this.steps = steps;
            this.simulate = simulate;
            this.aggregate = aggregate;
            this.getScore = getScore;
        }

        public IEnumerable<Solution> GetSolutions(State state, Countdown time)
        {
            var solutions = new List<(Solution, double)>();
            var bestScore = double.MinValue;

            foreach (var firstCarMove in PossibleMoves)
            foreach (var secondCarMove in PossibleMoves)
            {
                var solution = simulate == SimulateBy.Repeat
                    ? new Solution(
                        Enumerable.Repeat(firstCarMove, steps),
                        Enumerable.Repeat(secondCarMove, steps)
                    )
                    : new Solution(
                        Enumerable.Repeat(V.Zero, steps-1).Prepend(firstCarMove),
                        Enumerable.Repeat(V.Zero, steps-1).Prepend(secondCarMove)
                    );
                
                var score = Emulate(state, solution);
                solutions.Add((solution, score));
            }

            return solutions.OrderBy(x => x.Item2).Select(x => x.Item1);
        }

        private double Emulate(State state, Solution solution)
        {
            var copy = state.Copy();
            var stepsLeft = steps;
            var score = double.MinValue;
            var curSolution = solution;

            while (stepsLeft > 0)
            {
                copy.Tick(curSolution);
                curSolution = solution.NextTick();
                
                var newScore = getScore(copy);

                if (aggregate == AggregateBy.Last)
                    score = newScore;
                else if (aggregate == AggregateBy.Max && newScore > score)
                    score = newScore;

                stepsLeft--;
            }

            return score;
        }

        private static List<V> PossibleMoves => new List<V>
        {
            new V(-1, 1), new V(0, 1), new V(1, 1),
            new V(-1, 0), new V(0, 0), new V(1, 0),
            new V(-1, -1), new V(0, -1), new V(1, -1)
        };
    }

    public enum SimulateBy { DoNothing, Repeat }

    public enum AggregateBy { Max, Last }
}