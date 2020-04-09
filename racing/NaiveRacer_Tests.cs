using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AiAlgorithms.racing
{
    [TestFixture]
    public class NaiveRacer_Tests
    {
        [Test]
        [Explicit("Тест для отладки и анализа")]
        public void VisualizeRace([Values(0)]int testIndex)
        {
            // Открой файл bin/Debug/*/racing/visualizer/index.html чтобы посмотреть реплей на тесте testIndex
            var racer = new NaiveRacer();
            var test = RaceProblemsRepo.GetTests(false).ElementAt(testIndex);
            RaceController.Play(test, racer, true);
            Console.WriteLine(Path.Combine(TestContext.CurrentContext.TestDirectory, "racing", "visualizer", "index.html"));
        }
    }
}