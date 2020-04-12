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
        public void Play() => Play(new GreedySolver(), StateGenerator.Generate(new Random()));

        [Test]
        public void PlayAndSave() => Play(new GreedySolver(), StateGenerator.Generate(new Random()), "race");

        [TestCase(10, 100, 10, 1, 0, TestName = "Без препятствий")]
        [TestCase(10, 100, 10, 2, 0, TestName = "Мало препятствий")]
        [TestCase(10, 100, 10, 5, 0, TestName = "Больше препятствий")]
        [TestCase(10, 100, 10, 10, 0, TestName = "Много препятствий")]
        public void PlayTestGroup(int testsCount, int fieldSize, int flagsCount, int repeats, int obstaclesCount)
        {
            var random = new Random();
            
            for (var i = 0; i < testsCount; i++)
            {
                var state = StateGenerator.Generate(random, fieldSize, flagsCount, repeats, obstaclesCount);
                var solver = new GreedySolver();
                var saveFile = TestContext.CurrentContext.Test.Name + i;
                
                Play(solver, state, saveFile);
            }
        }

        private void Play(ISolver solver, State state, string saveFile=null)
        {
            var result = Controller.PlayToEnd(state, solver, saveFile);

            if (saveFile != null)
                Console.Write($"VisualizationName: {saveFile} - ");
            Console.WriteLine($"Time:{result.Time} - Flags:{result.FlagsTaken}");
        }
    }
}