using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;
using AI_Research_1.Solvers;
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

            stat.Select(x => (x.Key, x.Value)).OrderByDescending(x => x.Value.Mean).ToList()
                .ForEach(x => Console.Write($"{new FileInfo(x.Key).Name}\n\n{stat[x.Key]}\n\n"));
        }

        [Test, Explicit]
        public void CollectStatToCsv()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var statScore = new Dictionary<string, Stat>();
            var statCarsAlive = new Dictionary<string, Stat>();
            var statMaxTime = new Dictionary<string, Stat>();
            var projectDirectory = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "Statistics");
            var files = Directory.GetFiles(projectDirectory);
            foreach (var file in files)
            {
                var args = new FileInfo(file).Name.Split('.');
                var testName = args[1];
                var solverName = args[0];
                if (!statScore.ContainsKey(testName))
                    statScore[testName] = new Stat();
                if (!statCarsAlive.ContainsKey(testName))
                    statCarsAlive[testName] = new Stat();
                if (!statMaxTime.ContainsKey(testName))
                    statMaxTime[testName] = new Stat();
                var currentStatScore = statScore[testName];
                var currentAliveStatScore = statCarsAlive[testName];
                var currentMax = statMaxTime[testName];
                StatValue currentScoreStatValue = new StatValue();
                StatValue currentAliveStatValue = new StatValue();
                StatValue currentMaxTimeValue = new StatValue();
                switch (solverName)
                {
                    case nameof(HillClimbingSolver):
                        currentScoreStatValue = currentStatScore.HillClimbing;
                        currentAliveStatValue = currentAliveStatScore.HillClimbing;
                        currentMaxTimeValue = currentMax.HillClimbing;
                        break;
                    case nameof(RandomSolver):
                        currentScoreStatValue = currentStatScore.Random;
                        currentAliveStatValue = currentAliveStatScore.Random;
                        currentMaxTimeValue = currentMax.Random;
                        break;
                    case nameof(GreedySolver):
                        currentScoreStatValue = currentStatScore.Greedy;
                        currentAliveStatValue = currentAliveStatScore.Greedy;
                        currentMaxTimeValue = currentMax.Greedy;
                        break;
                    case "EvolutionSolver":
                        currentScoreStatValue = currentStatScore.Evolution;
                        currentAliveStatValue = currentAliveStatScore.Evolution;
                        currentMaxTimeValue = currentMax.Evolution;
                        break;
                }

                using TextReader stream = File.OpenText(file);
                stream
                    .ReadToEnd()
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Split(','))
                    .Select(x => GetFinalScore(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2]), int.Parse(x[3])))
                    .ToList()
                    .ForEach(x => currentScoreStatValue.Add(x));

                using TextReader sstream = File.OpenText(file);
                sstream
                    .ReadToEnd()
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Split(','))
                    .Select(x => int.Parse(x[4]))
                    .ToList()
                    .ForEach(x => currentAliveStatValue.Add(x));

                using TextReader ssstream = File.OpenText(file);
                ssstream
                    .ReadToEnd()
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Split(','))
                    .Select(x => int.Parse(x[3]))
                    .ToList()
                    .ForEach(x => currentMaxTimeValue.Add(x));
            }

            FormCsv(statScore, statCarsAlive, statMaxTime);
        }

        private void FormCsv(Dictionary<string, Stat> statScore, Dictionary<string, Stat> statCarsAlive,
            Dictionary<string, Stat> statMaxTime)
        {
            var csv = new StringBuilder();
            csv.Append(
                "Algorithm, Greedy, , Random, , Hill Climb, , Evolution, ,,Greedy, , Random, , Hill Climb, , Evolution, ,,Greedy, Random, Hill Climb, Evolution");
            csv.Append(Environment.NewLine);

            csv.Append(
                ", Mean,Sigma,Mean,Sigma,Mean,Sigma,Mean,Sigma, ,Mean,Sigma,Mean,Sigma,Mean,Sigma,Mean,Sigma, ,Max,Max,Max,Max");
            csv.Append(Environment.NewLine);

            var tests = statScore.Keys.ToList();
            var directory = Path.Combine(Environment.CurrentDirectory, "..", "..", "..");
            var file = Path.Combine(directory, "table.csv");
            if (File.Exists(file))
                File.Delete(file);
            var writer = File.AppendText(file);
            writer.Write(csv.ToString());
            foreach (var test in tests)
            {
                var score = statScore[test];
                var alive = statCarsAlive[test];
                var max = statMaxTime[test];
                var scoreStr = GetLineMeanSigma(score);
                var carsAlive = GetLineMeanSigma(alive);
                var maxStr = GetLineMax(max);
                var resStr = $"{test} ,{scoreStr}, ,{carsAlive}, ,{maxStr}";
                writer.WriteLine(resStr);
                writer.Flush();
                csv.Append(resStr);
                csv.Append(Environment.NewLine);
            }
        }

        private static string GetLineMeanSigma(Stat stat)
        {
            return
                $"{Math.Round(stat.Greedy.Mean, 2)},{Math.Round(stat.Greedy.StdDeviation, 2)},{Math.Round(stat.Random.Mean, 2)},{Math.Round(stat.Random.StdDeviation, 2)},{Math.Round(stat.HillClimbing.Mean, 2)},{Math.Round(stat.HillClimbing.StdDeviation, 2)},{Math.Round(stat.Evolution.Mean, 2)},{Math.Round(stat.Evolution.StdDeviation, 2)}";
        }

        private static string GetLineMax(Stat stat)
        {
            return
                $"{stat.Greedy.Max},{stat.Random.Max},{stat.HillClimbing.Max},{stat.Evolution.Max}";
        }


        private static State PlayToEnd(ISolver solver, State state, bool saveReplay, bool saveStats)
        {
            var testName = TestContext.CurrentContext.Test.Name.Split("_")[0];
            var replayFile = !saveReplay
                ? null
                : $"{solver.GetNameWithArgs()}.{Emulator.GetScore.Method.Name}_{DateTime.Now:dd.HH.mm.ss}.js";
            var statsFile = !saveStats
                ? null
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