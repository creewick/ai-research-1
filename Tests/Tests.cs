using System;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;
using AI_Research_1.Solvers;
using NUnit.Framework;

namespace AI_Research_1.Tests
{
    [TestFixture]
    public class Tests
    {
        /* Чтобы визуализировать тест:
         *
         * 1. Укажи параметр saveFile в Controller.PlayToEnd
         * 2. Найти файл с этим именем в ai-research-1/Visualization
         * 3. Переименуй в race.js
         * 4. Открой index.html
         */
        
        [Test]
        public void Play() => Play(new HillClimbingSolver(), StateGenerator.Generate(new Random()));

        [Test]
        public void PlayAndSave() => Play(new HillClimbingSolver(), StateGenerator.Generate(new Random()), "race");

        [TestCase(10, 100, 10, 1, 0, TestName = "0-blocks")]
        [TestCase(10, 100, 10, 1, 5, TestName = "5-blocks")]
        [TestCase(10, 100, 10, 1, 10, TestName = "10-blocks")]
        [TestCase(10, 100, 10, 1, 20, TestName = "20-blocks")]
        public void PlayTestGroup(int testsCount, int fieldSize, int flagsCount, int repeats, int obstaclesCount)
        {
        
            var random = new Random();
            
            for (var i = 0; i < testsCount; i++)
            {
                var state = StateGenerator.Generate(random, fieldSize, flagsCount, repeats, obstaclesCount);
                var solver = new HillClimbingSolver();
                var testName = TestContext.CurrentContext.Test.Name;
                var saveFile = $"{DateTime.Now:dd.HH;mm;ss}_{testName}_{i}";
                
                Play(solver, state, saveFile);
            }
        }

        private void Play(ISolver solver, State state, string saveFile=null)
        {
            if (saveFile != null)
                Console.WriteLine($"FileName: {saveFile}.js");
            
            var result = Controller.PlayToEnd(state, solver, saveFile);
            
            Console.WriteLine($"Time: {result.Time} - Flags: {result.FlagsTaken}");
        }
    }
}