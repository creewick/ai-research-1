using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AiAlgorithms.Algorithms;
using NUnit.Framework;

namespace AiAlgorithms.racing
{
    public class RaceController
    {
        public RaceController(int timeoutPerTick = 100)
        {
            this.timeoutPerTick = timeoutPerTick;
        }

        private readonly int timeoutPerTick;

        public static RaceState Play(RaceState state, ISolver<RaceState, RaceSolution> racer, bool makeLog)
        {
            return new RaceController().PlayToEnd(state, racer, makeLog);
        }

        public RaceState PlayToEnd(RaceState state, ISolver<RaceState, RaceSolution> racer, bool makeLog)
        {
            if (!makeLog)
                return Play(state, racer, null);
            var filename = Path.Combine(
                TestContext.CurrentContext.TestDirectory,
                "racing",
                "visualizer",
                "last-log.js"
            );
            using var streamWriter = new StreamWriter(filename);
            var result = Play(state, racer, new JsonGameLogger(streamWriter));
            Console.WriteLine(result.Time + "\t" + result.Car);
            return result;
        }

        public IEnumerable<RaceState> Play(RaceState initialState, RaceSolution solution)
        {
            var race = initialState.MakeCopy();
            var i = 0;
            while (!race.IsFinished && i < solution.Accelerations.Length)
            {
                var command = solution.Accelerations[i++];
                race.Car.NextCommand = command;
                race.Tick();
                yield return race.MakeCopy();
            }
        }

        public StatValue ImprovementsCount { get; } = new StatValue();
        public StatValue TimeMs { get; } = new StatValue();
        public StatValue MutationIndex { get; } = new StatValue();
        public Dictionary<string, int> HintFrequencies { get; } = new Dictionary<string, int>();

        public override string ToString()
        {
            var hints = HintFrequencies.OrderByDescending(kv => kv.Value).Take(5).StrJoin("; ", kv => kv.Value + ":" + kv.Key);
            return $"ImprovementsCount: {ImprovementsCount}, TimeMs: {TimeMs}, MutationIndex: {MutationIndex}, {hints}";
        }

        public RaceState Play(RaceState initialState, ISolver<RaceState, RaceSolution> solver,
            IGameLogger<RaceTrack, RaceState> logger)
        {
            var race = initialState.MakeCopy();
            logger?.LogStart(race.Track);
            while (!race.IsFinished)
            {
                var variants = solver.GetSolutions(race.MakeCopy(), timeoutPerTick).ToList();
                var aiLogger = logger?.GetAiLogger(0);
                LogAiVariants(race, aiLogger, variants);
                var lastSolution = variants.Last();
                HintFrequencies[lastSolution.Hint ?? ""] = HintFrequencies.GetOrDefault(lastSolution.Hint ?? "") + 1;
                var command = lastSolution.Accelerations[0];
                race.Car.NextCommand = command;
                ImprovementsCount.Add(lastSolution.ImprovementIndex);
                MutationIndex.Add(lastSolution.MutationIndex);
                TimeMs.Add(lastSolution.Time.TotalMilliseconds);
                logger?.LogTick(race);
                race.Tick();
            }
            logger?.LogEnd(race);
            return race;
        }

        private void LogAiVariants(RaceState state, IGameAiLogger aiLogger, List<RaceSolution> variants)
        {
            var variantsToDraw = 9;
            var variantsToLog = variants.Cast<RaceSolution>().Reverse().Take(variantsToDraw).ToList();
            var log = variantsToLog.Select(v => $"{v.Score.ToString(CultureInfo.InvariantCulture)} {v.Accelerations.StrJoin(",")} {v.ImprovementIndex} of {v.MutationIndex} in {v.Time} {v.Hint}").StrJoin("\n");
            aiLogger?.LogText(log);

            foreach (var solution in variantsToLog.Skip(1))
            {
                LogSolution(solution, state, aiLogger, 0.1);
            }
            LogSolution(variantsToLog.First(), state, aiLogger, 1);
        }

        private static void LogSolution(RaceSolution solution, RaceState state, IGameAiLogger aiLogger, double intensity)
        {
            var state2 = state.MakeCopy();
            var points = new List<V>();
            foreach (var a in solution.Accelerations)
            {
                var start = state2.Car.Pos;
                state2.Car.NextCommand = a;
                state2.Tick();
                var end = state2.Car.Pos;
                points.Add(start);
                points.Add(end);
            }
            aiLogger?.LogLine(points, intensity);
        }
    }
}