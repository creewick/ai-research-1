using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace AiAlgorithms
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var result = RunTest(args);
            Console.Write(JsonSerializer.Serialize(result));
        }

        private static RunResult RunTest(string[] args)
        {
            try
            {
                var newOut = new StringWriter();
                var oldOut = Console.Out;
                Console.SetOut(newOut);
                try
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var scoringTest = (IScoredTest) Activator.CreateInstance(assembly.GetType(args[0]));
                    return GetRunResult(scoringTest, newOut);
                }
                finally
                {
                    Console.SetOut(oldOut);
                }
            }
            catch (Exception e)
            {
                return new RunResult(Verdict.RuntimeError, e.ToString());
            }
        }

        private static RunResult GetRunResult(IScoredTest scoringTest, StringWriter newOut)
        {
            var score = scoringTest.CalculateScore();
            var output = $"Your score is {score.ToString(CultureInfo.InvariantCulture)}";
            if (score >= scoringTest.MinScoreToPassTest)
            {
                return new RunResult(Verdict.Ok, $"{output}\n\n{newOut}", score);
            }
            return new RunResult(Verdict.RuntimeError, $"{output} is too low. Gain at least {scoringTest.MinScoreToPassTest}!\n\n{newOut}", score);
        }
    }
    public enum Verdict
    {
        NA = 0,
        Ok = 1, // Означает, что всё штатно протестировалось. Возвращается в том числе если тесты не прошли
        CompilationError = 2,
        RuntimeError = 3,
        SecurityException = 4,
        SandboxError = 5,
        OutputLimit = 6,
        TimeLimit = 7,
        MemoryLimit = 8
    }

    public class RunResult
    {
        public Verdict Verdict { get; set; }
        public string Output { get; set; }
        public double? Points { get; set; }

        public RunResult(Verdict verdict, string output = "", double? points = null)
        {
            Verdict = verdict;
            Output = output ?? ""; 
            Points = points;
        }
    }

    public interface IScoredTest
    {
        double CalculateScore();
        double MinScoreToPassTest { get; }
    }
}