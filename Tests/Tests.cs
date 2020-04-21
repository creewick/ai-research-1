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
        private const bool SaveReplay = true;
        private const bool SaveStats = true;
        private const int RepeatCount = 10;
        
        private static readonly List<ISolver> Solvers = new List<ISolver>
        {
            new GreedySolver(20),
            new RandomSolver(10, 8),
            new HillClimbingSolver(),
            new EvolutionSolver()
        };

        [TestCaseSource(nameof(TestCases))]
        public void Play(State state, ISolver solver)
        {
            PlayToEnd(solver, state, SaveReplay, SaveStats);
        }   
        private long getScore(int flagsGoal, int trackTime, int flagsTaken, int time)
        {
            const int flagCoef = 100;
            return (trackTime - time) + (flagsTaken - flagsGoal) * flagCoef;
        }

        [Test]
        public void CollectStats()
        {
            var stat = new Dictionary<string, StatValue>();
            var projectDirectory = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "Statistics");
            var files = Directory.GetFiles(projectDirectory);
            foreach (var file in files)
            {
                stat[file] = new StatValue();
                using (TextReader stream = File.OpenText(file))
                {
                    var lines = stream.ReadToEnd()
                        .Split(new char[]{'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Split(','));
                    var scores = lines
                        .Select(x =>
                            getScore(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2]), int.Parse(x[3])))
                        .ToList();
                    scores.ForEach(x => stat[file].Add(x));
                }
            }
        }
        
        private static State PlayToEnd(ISolver solver, State state, bool saveReplay, bool saveStats)
        {
            var replayFile = !saveReplay ? null
                : $"{solver.GetNameWithArgs()}_{DateTime.Now:dd.HH.mm.ss}.js";
            var statsFile = !saveStats ? null
                : $"{solver.GetNameWithArgs()}.txt";
            
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
                .Select(x => new {State = (State) x.GetValue(null), Name = x.Name});

            for (var i = 0; i < RepeatCount; i++)
            foreach (var stateObj in states)
            foreach (var solver in Solvers)
                yield return new TestCaseData(stateObj.State, solver)
                    .SetName($"{stateObj.Name}_{solver.GetType().Name}_{i}");
        }
    }
}