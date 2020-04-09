using System;
using System.Collections.Generic;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.racing
{
    public class GreedyRacer : ISolver<RaceState, RaceSolution>
    {
        public IEnumerable<RaceSolution> GetSolutions(RaceState problem, Countdown countdown)
        {
            yield break;
        }
    }
}