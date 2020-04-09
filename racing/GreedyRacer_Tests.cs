using System;
using System.Linq;
using NUnit.Framework;

namespace AiAlgorithms.racing
{
    [TestFixture]
    public class GreedyRacer_Tests : IScoredTest
    {
        public double MinScoreToPassTest => 9200;

        [Test]
        public void QualityIsOK()
        {
            var totalScore = CalculateScore();
            Console.WriteLine($"Total score is {totalScore}");
            Assert.That(totalScore, Is.GreaterThan(MinScoreToPassTest));
        }

        [Test]
        [Explicit("Тест для отладки и анализа")]
        public void VisualizeRace()
        {
            // Открой файл bin/Debug/*/racing/visualizer/index.html чтобы посмотреть реплей на тесте testIndex
            var greedyRacer = new GreedyRacer();
            var test = RaceProblemsRepo.GetTests(false).ElementAt(0);
            RaceController.Play(test, greedyRacer, true);
        }

        public double CalculateScore()
        {
            var greedyRacer = new GreedyRacer();
            return RacingEvaluator.GetTotalScore(greedyRacer, 100, true, RaceProblemsRepo.GetTests(false));
        }
    }
}