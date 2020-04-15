using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;
using AI_Research_1.Solvers;
using NUnit.Framework;

namespace AI_Research_1.Tests
{
    [TestFixture]
    public class Tests
    {
        private ISolver solver = new HillClimbingSolver();
        private List<ISolver> solvers = new List<ISolver>() {new GreedySolver(), new HillClimbingSolver()};

        /* Чтобы визуализировать тест:
         *
         * 1. Укажи параметр saveFile в Controller.PlayToEnd
         * 2. Найти файл с этим именем в ai-research-1/Visualization
         * 3. Переименуй в race.js
         * 4. Открой index.html
         */

        [Test]
        public void Play() => Play(solver, StateGenerator.Generate(new Random()));

        [Test]
        public void PlayAndSave() => Play(solver, StateGenerator.Generate(new Random()), "race");

        [TestCase(10, 100, 10, 1, 0, TestName = "0-blocks")]
        [TestCase(10, 100, 10, 1, 5, TestName = "5-blocks")]
        [TestCase(10, 100, 10, 1, 10, TestName = "10-blocks")]
        [TestCase(10, 100, 10, 1, 20, TestName = "20-blocks")]
        public void PlayTestGroup(int testsCount, int fieldSize, int flagsCount, int repeats, int obstaclesCount)
        {
            var random = new Random();
            var statistics = new Dictionary<ISolver, Stat>();
            solvers.ForEach(x => statistics[x] = new Stat());
            var states = Enumerable.Range(0, testsCount).Select(x =>
                StateGenerator.Generate(random, fieldSize, flagsCount, repeats, obstaclesCount)).ToList();
            foreach (var testingSolver in solvers)
            {
                var statFlags = new StatValue();
                var statTicks = new StatValue();
                
                for (var i = 0; i < testsCount; i++)
                {
                    var state = states[i];
                    var testName = TestContext.CurrentContext.Test.Name;
                    var saveFile = $"{DateTime.Now:dd.HH;mm;ss}_{testName}_{i}";
                    var result = Play(testingSolver, state, saveFile);
                    statFlags.Add(result.FlagsTaken);
                    statTicks.Add(result.Time);
                }

                statistics[testingSolver].FlagsCountStat = statFlags;
                statistics[testingSolver].TicksCountStat = statTicks;
            }

            foreach (var solver in solvers)
            {
                var testName = TestContext.CurrentContext.Test.Name;
                Console.WriteLine(solver.GetType());
                Console.WriteLine(testName);
                Console.WriteLine($"flags count mean {statistics[solver].FlagsCountStat.Mean}");
                Console.WriteLine(
                    $"flags count conf. Interval size {statistics[solver].FlagsCountStat.ConfIntervalSize}");
                Console.WriteLine($"ticks count mean {statistics[solver].TicksCountStat.Mean}");
                Console.WriteLine(
                    $"ticks count conf. Interval size {statistics[solver].TicksCountStat.ConfIntervalSize}");
            }
        }

        private State Play(ISolver solver, State state, string saveFile = null)
        {
            if (saveFile != null)
                Console.WriteLine($"FileName: {saveFile}.js");

            var result = Controller.PlayToEnd(state, solver, saveFile);

            Console.WriteLine($"Time: {result.Time} - Flags: {result.FlagsTaken}");
            return result;
        }
    }
}