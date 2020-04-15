using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;
using AI_Research_1.Solvers;
using AI_Research_1.Solvers.Evolution;
using AI_Research_1.Solvers.Evolution.BaseSolvers;
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
        private List<ISolver> solvers;

        private List<State> states;

        [OneTimeSetUp]
        public void SetUp()
        {
            var random = new Random();
            
            solvers = new List<ISolver>
            {
                new GreedySolver(),
                new RandomSolver(),
                new HillClimbingSolver(),
                new EvolutionSolver()
            };
            states = Enumerable
                .Range(0, 5)
                .Select(i => StateGenerator.Get(random, 100, 10, 1, 0))
                .ToList();
        }


        [Test]
        public void PlayOne() => Play(solvers[0], states[0]);

        [Test]
        public void PlayOneAndSave() => Play(solvers[0], states[0], "race");

        [Test]
        public void PlayAll()
        {
            Console.SetOut(TestContext.Progress);
            var stats = new Dictionary<ISolver, Stat>();
            solvers.ForEach(x => stats[x] = new Stat());
            
            foreach (var solver in solvers)
            {
                var statFlags = new StatValue();
                var statTicks = new StatValue();
                
                for (var i = 0; i < states.Count; i++)
                {
                    var result = Play(solver, states[i]);
                    statFlags.Add(result.FlagsTaken);
                    statTicks.Add(result.Time);
                }

                stats[solver].FlagsStat = statFlags;
                stats[solver].TimeStat = statTicks;
            }
            
            Console.WriteLine($"Results of{states.Count} tests:\n");
            solvers.ForEach(s => Console.WriteLine(
                $"Solver: {s.GetType()} Flags: {stats[s].FlagsStat} Time: {stats[s].TimeStat}"));
        }
        private static State Play(ISolver solver, State state, string saveFile = null)
        {
            if (saveFile != null)
                Console.WriteLine($"FileName: {saveFile}.js");

            var result = Controller.PlayToEnd(state, solver, saveFile);

            Console.WriteLine($"Solver: {solver.GetType()} Time: {result.Time} Flags: {result.FlagsTaken}");
            return result;
        }
    }
}