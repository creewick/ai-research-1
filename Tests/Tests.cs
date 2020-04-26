using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;
using AI_Research_1.Solvers;
using AI_Research_1.Solvers.Evolution;
using AI_Research_1.Solvers.HillClimbing;
using NUnit.Framework;

namespace AI_Research_1.Tests
{
    /* Чтобы визуализировать тест:
     *
     * 1. Укажи параметр saveFile в Controller.PlayToEnd
     * 2. Найти файл с этим именем в ai-research-1/Visualization
     * 3. Переименуй в race.js
     * 4. Открой index.html
     */
    [TestFixture]
    public class Tests
    {
        private const bool SaveReplay = false;
        private const bool SaveStats = true;
        private const int RepeatCount = 20;

        private static readonly List<ISolver> Solvers = new List<ISolver>
        {
            new GreedySolver(20),
            new RandomSolver(16, 6),
            new HillClimbingSolver()
        };

        [Timeout(60000)]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(nameof(TestCases))]
        public void Play(State state, ISolver solver)
        {
            PlayToEnd(solver, state, SaveReplay, SaveStats);
        }   
        
        private static long GetFinalScore(int flagsGoal, int trackTime, int flagsTaken, int time)
        {
            int flagCoef = trackTime / flagsGoal;
            return (trackTime - time) + (flagsTaken - flagsGoal) * flagCoef;
        }

        [Test, Explicit]
        public void CheckStats()
        {
            var stat = new Dictionary<string, StatValue>();
            var projectDirectory = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "Statistics");
            var files = Directory.GetFiles(projectDirectory);
            foreach (var file in files)
            {
                stat[file] = new StatValue();
                using TextReader stream = File.OpenText(file);
                stream
                    .ReadToEnd()
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Split(','))
                    .Select(x => int.Parse(x[4]))
                    .ToList()
                    .ForEach(x => stat[file].Add(x));
                
            }
            stat.Select(x=>(x.Key,x.Value)).OrderByDescending(x=>x.Value.Mean).ToList().ForEach(x=>Console.Write($"{new FileInfo(x.Key).Name}\n\n{stat[x.Key]}\n\n"));
        }
        
        private static State PlayToEnd(ISolver solver, State state, bool saveReplay, bool saveStats)
        {
            var testName = TestContext.CurrentContext.Test.Name.Split("_")[0];
            var replayFile = !saveReplay ? null
                : $"{solver.GetNameWithArgs()}.{Emulator.GetScore.Method.Name}_{DateTime.Now:dd.HH.mm.ss}.js";
            var statsFile = !saveStats ? null
                : $"{solver.GetType().Name}.{testName}.txt";
            
            var result = Controller.PlayToEnd(state, solver, replayFile, statsFile);
            
            Console.WriteLine($"Time: {result.Time} Flags: {result.FlagsTaken}\n");

            if (saveReplay)
                Console.WriteLine($"Replay saved to: {replayFile}");
            if (saveStats)
                Console.WriteLine($"Stats saved to: {statsFile}");

            return result;
        }

        private static IEnumerable TestCases()
        {
            var states = typeof(TestStates)
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.PropertyType == typeof(State))
                .Select(x => new {State = (State) x.GetValue(null), Name = x.Name})
                .ToList();

            for (var i = 0; i < RepeatCount; i++)
                foreach (var stateObj in states)
                    for (var j = 0; j < Solvers.Count; j++)
                        yield return new TestCaseData(stateObj.State, Solvers[j])
                            .SetName($"{stateObj.Name}_{Solvers[j].GetType().Name}_{j}_Test_{i}");
        }
    }
}