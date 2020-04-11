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
        
        [Test]
        public void Play()
        {
            ISolver solver = new GreedySolver();

            var state = States.Generate(new Random());

            var result = Controller.PlayToEnd(state, solver, false);
            
            Console.WriteLine($"Time:{result.Time} - Flags:{result.FlagsTaken}");
        }
    }
}