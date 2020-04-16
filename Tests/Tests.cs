using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;
using AI_Research_1.Solvers;
using AI_Research_1.Solvers.Evolution;
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
        
        private static readonly List<ISolver> Solvers = new List<ISolver>
        {
            new GreedySolver(20),
            new UniversalRandomSolver(10, 8, AggregateBy.Max, Emulator.DefaultGetScore, true),
            new HillClimbingSolver(),
            new EvolutionSolver()
        };

        [TestCaseSource(nameof(TestCases))]
        public void Play(State state, ISolver solver)
        {
            PlayToEnd(solver, state, SaveReplay, SaveStats);
        }   
        
        private static State PlayToEnd(ISolver solver, State state, bool saveReplay, bool saveStats)
        {
            var solverArgs = GetSolverArgs(solver);
            var replayFile = !saveReplay ? null
                : $"{solver.GetType().Name}_{DateTime.Now:dd.HH.mm.ss}";
            var statsFile = !saveStats ? null
                : FormatArgsFileName(solver, solverArgs);
            
            var result = Controller.PlayToEnd(state, solver, replayFile, statsFile);
            
            Console.WriteLine($"{FormatArgsLine(solver, solverArgs)}\n");
            Console.WriteLine($"Time: {result.Time} Flags: {result.FlagsTaken}\n");

            if (saveReplay)
                Console.WriteLine($"Replay saved to: {replayFile}.js");
            if (saveStats)
                Console.WriteLine($"Stats saved to: {statsFile}.js");

            return result;
        }


        private static IEnumerable TestCases()
        {
            var states = typeof(TestStates)
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.PropertyType == typeof(State))
                .Select(x => new {State = (State) x.GetValue(null), Name = x.Name});

            foreach (var stateObj in states)
            foreach (var solver in Solvers)
                yield return new TestCaseData(stateObj.State, solver)
                    .SetName($"{stateObj.Name}_{solver.GetType().Name}");
        }

        private static FieldInfo[] GetSolverArgs(ISolver solver) => solver
            .GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

        private static string FormatArgsLine(ISolver solver, FieldInfo[] args) => 
            string.Join(" ", args.Select(a => $"{a.Name}: {GetValue(a, solver)}"));

        private static string FormatArgsFileName(ISolver solver, FieldInfo[] args) =>
            $"{solver.GetType().Name}.{string.Join(".", args.Select(a => GetValue(a, solver)))}";
        
        private static string GetValue(FieldInfo field, ISolver solver)
        {
            var value = field.GetValue(solver);
            
            if (value is Func<State, long> func)
                return func.Method.Name;
            
            return value?.ToString();
        }
    }
}