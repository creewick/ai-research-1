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

        public static readonly Func<State, long> GetScore = GetScore_3;

        public static IEnumerable<Solution> SortByScore(IEnumerable<Solution> solutions, State state)
        {
            return solutions
                .OrderByDescending(s => 
                    Emulate(state, s, s.FirstCarCommandsList.Count()));
        }

        public static long Emulate(State state, Solution solution, int steps, AggregateBy aggregate = AggregateBy.Max, Func<State, long> getScore=null)
        {
            getScore ??= GetScore;
            var copy = state.Copy();
            var score = long.MinValue;
            var currentSolution = solution;

            for (var i = 0; i < steps; i++)
            {
                copy.Tick(currentSolution);
                currentSolution = solution.GetNextTick();
                
                var newScore = getScore(copy);

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

        public static long GetScore_1(State state) => 
            + IsAlive(state)
            + FlagsTaken(state)
            - state.GetNextFlag().Dist2To(state.FirstCar.Pos)
            - state.GetNextFlag().Dist2To(state.SecondCar.Pos);

        public static long GetScore_2(State state)
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

        public static long GetScore_3(State state)
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
        public static long GetScore_4(State state)
        {
            var flagsCount = 3;
            var cars = new List<Car> {state.FirstCar, state.SecondCar};
            var nextFlags = state.GetNextNFlags(flagsCount).ToList();
            var distancesCost =
                nextFlags
                    .Select(flag =>
                    {
                        return cars
                            .Select(car => car.Pos.Dist2To(flag))
                            .Min();
                    })
                    .Sum();
            var additionalDistanceCost = cars
                .Select(car => nextFlags
                    .Select(flag => car.Pos.Dist2To(flag))
                    .Min())
                .Sum();
            return FlagsTaken(state) + IsAlive(state) - distancesCost - additionalDistanceCost;
        }

        public static long GetScore_5(State state)
        {
            var flagsCount = 2;
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
        
        /* Предсказывать игру на несколько флагов вперед (эвристика, ослабленные правила)
         * Каждый тик - передвижение с единичной скоростью, без ускорения
         *
         * Посчитать время, требующееся на сбор 5 флагов
         */
    }
}