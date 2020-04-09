using System;
using System.Collections.Generic;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.racing
{
    public class RacingEvaluator
    {
        public static double GetTotalScore(ISolver<RaceState, RaceSolution> racer, int timeoutMs,
            bool logScore, IEnumerable<RaceState> tests)
        {
            var score = 0.0;
            int iTest = 0;
            foreach (var test in tests)
            {
                var finalState = RaceController.Play(test, racer, false);
                var testScore = GetScore(finalState);
                if (logScore)
                    Console.WriteLine($"Test #{iTest} score: {testScore} (flags: {finalState.Car.FlagsTaken} of {test.Track.FlagsToTake}, time: {finalState.Time} of {test.Track.RaceDuration})");
                score += testScore;
                iTest++;
            }
            return score;
        }

        public static int GetScoreNew(RaceState finalState)
        {
            return finalState.Track.RaceDuration - finalState.Time - (finalState.Track.FlagsToTake - finalState.Car.FlagsTaken) * 100;
        }

        public static int GetScore(RaceState finalState)
        {
            return finalState.Car.FlagsTaken * 100 - finalState.Time;
        }
    }
}