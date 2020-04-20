using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers
{
    public enum AggregateBy { Max, Last, Sum }

    public static class Emulator
    {
        private const long FlagCost = 100000;
        private static long IsAlive(State state) => state.FirstCar.IsAlive && state.SecondCar.IsAlive ? FlagCost * 1000 : 0;
        private static long FlagsTaken(State state) => FlagCost * state.FlagsTaken;

        private static readonly Func<State, long> GetScore = GetScore_3;

        public static IEnumerable<Solution> SortByScore(IEnumerable<Solution> solutions, State state)
        {
            return solutions
                .OrderByDescending(s => 
                    Emulate(state, s, s.FirstCarCommandsList.Count()));
        }

        public static long Emulate(State state, Solution solution, int steps, AggregateBy aggregate = AggregateBy.Max)
        {
            var copy = state.Copy();
            var score = long.MinValue;
            var currentSolution = solution;

            for (var i = 0; i < steps; i++)
            {
                copy.Tick(currentSolution);
                currentSolution = solution.GetNextTick();
                
                var newScore = GetScore(copy);

                switch (aggregate)
                {
                    case AggregateBy.Last:
                        score = newScore;
                        break;
                    case AggregateBy.Max when newScore > score:
                        score = newScore;
                        break;
                    case AggregateBy.Sum:
                        score += newScore;
                        break;
                }
            }

            return score;
        }

        private static long GetScore_1(State state) => 
            + IsAlive(state)
            + FlagsTaken(state)
            - state.GetNextFlag().Dist2To(state.FirstCar.Pos)
            - state.GetNextFlag().Dist2To(state.SecondCar.Pos);

        private static long GetScore_2(State state)
        {
            var taken = state.FlagsTaken;
            var all = state.Track.Flags.Count;
            var firstCarFlag = state.Track.Flags[(taken + taken % 2) % all];
            var secondCarFlag = state.Track.Flags[(taken + 1 - taken % 2) % all];

            return FlagsTaken(state)
                   + IsAlive(state)
                   - firstCarFlag.Dist2To(state.FirstCar.Pos)
                   - secondCarFlag.Dist2To(state.SecondCar.Pos);
        }

        private static long GetScore_3(State state)
        {
            var flags = new[]
                {
                    state.GetNextFlag(),
                    state.GetNextNextFlag()
                }
                .OrderBy(v => state.FirstCar.Pos.Dist2To(v));

            return FlagsTaken(state)
                   + IsAlive(state)
                   - flags.First().Dist2To(state.FirstCar.Pos)
                   - flags.Last().Dist2To(state.SecondCar.Pos);
        }
        private static long GetScore_4(State state, int flagsCount = 3)
        {
            var cars = new List<Car> {state.FirstCar, state.SecondCar};
            var distancesCost =
                state.GetNextNFlags(flagsCount)
                    .Select(flag => cars
                        .Select(car => car.Pos.Dist2To(flag))
                        .Min()
                    )
                    .Sum();

            return FlagsTaken(state) + IsAlive(state) - distancesCost;
        }

        private static long GetScore_5(State state, int flagsCount)
        {
            var coefficient = (int)Math.Pow(2, flagsCount + 1);
            var cars = new List<Car> {state.FirstCar, state.SecondCar};
            var distancesCost =
                state.GetNextNFlags(flagsCount)
                    .Select(flag => cars
                        .Select(car =>
                        {
                            coefficient /= 2;
                            return car.Pos.Dist2To(flag) * coefficient;
                        })
                        .Sum()
                    )
                    .Sum();
            
            return FlagsTaken(state) + IsAlive(state) - distancesCost;
        }
    }
}