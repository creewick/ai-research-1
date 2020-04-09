using System;
using System.Collections.Generic;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.racing
{
    public class NaiveRacer : ISolver<RaceState, RaceSolution>
    {
        public IEnumerable<RaceSolution> GetSolutions(RaceState problem, Countdown countdown)
        {
            var delta = problem.GetFlagFor(problem.Car) - problem.Car.Pos;
            yield return new RaceSolution(new[] {new V(Math.Sign(delta.X), Math.Sign(delta.Y))});
        }
    }
}