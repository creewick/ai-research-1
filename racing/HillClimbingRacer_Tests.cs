using System;
using System.Linq;
using NUnit.Framework;

namespace AiAlgorithms.racing
{
    [TestFixture]
    public class HillClimbingRacer_Tests : IScoredTest
    {
        public double MinScoreToPassTest => 3000;

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
            var racer = new HillClimbingRacer();
            var test = RaceProblemsRepo.GetTests(true).ElementAt(0);
            RaceController.Play(test, racer, true);
        }

        [Explicit]
        [TestCase(1, "40,-1 6,-1 5")]
        public void DebugSingleTick(int testIndex, string car)
        {
            // Так можно отлаживать работу алгоритма.
            // Заметили странное поведение в визуализаторе - копируем сюда, изучаем все решения, отлаживаемся.
            var test = RaceProblemsRepo.GetTests(false).ElementAt(testIndex);
            var state = new RaceState(test.Track, Car.ParseCar(car));
            Console.WriteLine(state);
            var racer = new HillClimbingRacer();
            var solutions = racer.GetSolutions(state, 100).ToList();
            foreach (var solution in solutions)
            {
                Console.WriteLine(solution);
            }
        }

        public double CalculateScore()
        { 
            return RacingEvaluator.GetTotalScore(new HillClimbingRacer(), 100, true, RaceProblemsRepo.GetTests(true).Take(2));
        }
    }
}