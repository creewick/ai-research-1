using System;
using System.Collections;
using System.Collections.Concurrent;
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
        private static readonly string ProjectDirectory = Path.Combine(Environment.CurrentDirectory, "..", "..", "..");
        private const bool SaveReplay = false;
        private const bool SaveStats = true;
        private const int RepeatCount = 10;

        private static string GetSolverName(ISolver solver) => solver.GetNameWithArgs();

        private static readonly List<ISolver> Solvers = new List<ISolver>
        {
            new GreedySolver(10, Emulator.GetScore_1),
            new GreedySolver(10, Emulator.GetScore_2),
            new GreedySolver(10, Emulator.GetScore_3),
            new GreedySolver(10, Emulator.GetScore_4),
            new GreedySolver(10, Emulator.GetScore_5)
        };

        [Timeout(60000)]
        //[Parallelizable(ParallelScope.All)] do not use it!
        [TestCaseSource(nameof(TestCasesGood))]
        public void Play(State state, ISolver solver, int repeat, string groupName)
        {
            PlayToEnd(solver, state, SaveReplay, SaveStats, repeat, groupName);
        }

        private static State PlayToEnd(ISolver solver, State state, bool saveReplay, bool saveStats, int repeat,
            string groupName)
        {
            var testName = TestContext.CurrentContext.Test.Name.Split("_")[0];
            var replayFile = !saveReplay
                ? null
                : $"{solver.GetNameWithArgs()}.{Emulator.GetScore.Method.Name}_{DateTime.Now:dd.HH.mm.ss}.js";
            var statsFile = !saveStats
                ? null
                : $"{GetSolverName(solver)}.{groupName}.txt";
            if (statsFile != null)
            {
                if (!lastRepeats.ContainsKey(statsFile))
                    lastRepeats[statsFile] = -1;
                if (lastRepeats[statsFile] != repeat)
                {
                    var file = Path.Combine(ProjectDirectory, "Statistics", statsFile);
                    File.AppendAllText(file, "@\n");
                }

                lastRepeats[statsFile] = repeat;
            }

            var result = Controller.PlayToEnd(state, solver, replayFile, statsFile);

            Console.WriteLine($"Time: {result.Time} Flags: {result.FlagsTaken}\n");

            if (saveReplay)
                Console.WriteLine($"Replay saved to: {replayFile}");
            if (saveStats)
                Console.WriteLine($"Stats saved to: {statsFile}");

            return result;
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
            var files = Directory
                    // sorry, это чтобы на mac os всё работало
                    .GetFiles(projectDirectory)
                    .Where(fileName => !fileName.Contains("DS_Store"))
                    .ToList();
            foreach (var file in files)
            {
                stat[file] = new StatValue();
                using TextReader stream = File.OpenText(file);
                var repeats = stream
                    .ReadToEnd()
                    .Split('@');

                foreach (var repeat in repeats)
                {
                    var scores = repeat.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                        .Select(line => line.Split(','))
                        .Select(line =>
                            GetFinalScore(int.Parse(line[0]), int.Parse(line[1]), int.Parse(line[2]),
                                int.Parse(line[3])));
                    stat[file].Add(scores.Sum());
                }
            }

            stat.Select(x => (x.Key, x.Value))
                .OrderByDescending(x => x.Value.Mean)
                .ToList()
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
                $"{Math.Round(stat.Greedy.Mean, 2)},{Math.Round(stat.Greedy.ConfIntervalSize/2, 2)},{Math.Round(stat.Random.Mean, 2)},{Math.Round(stat.Random.ConfIntervalSize/2, 2)},{Math.Round(stat.HillClimbing.Mean, 2)},{Math.Round(stat.HillClimbing.ConfIntervalSize/2, 2)},{Math.Round(stat.Evolution.Mean, 2)},{Math.Round(stat.Evolution.ConfIntervalSize/2, 2)}";
        }

        private static string GetLineMax(Stat stat)
        {
            return
                $"{stat.Greedy.Max},{stat.Random.Max},{stat.HillClimbing.Max},{stat.Evolution.Max}";
        }

        private static ConcurrentDictionary<string, int> lastRepeats = new ConcurrentDictionary<string, int>();


        private static IEnumerable TestCasesGood()
        {
            var states = typeof(TestStatesGood)
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.PropertyType == typeof(State))
                .Select(x => new {State = (State) x.GetValue(null), Name = x.Name})
                .ToList();

            for (var i = 0; i < RepeatCount; i++)
                foreach (var stateObj in states)
                    for (var j = 0; j < Solvers.Count; j++)
                        yield return new TestCaseData(stateObj.State, Solvers[j], i, nameof(TestCasesGood))
                            .SetName($"{stateObj.Name}_{GetSolverName(Solvers[j])}_{j}_Test_{i}");
        }
    }
}