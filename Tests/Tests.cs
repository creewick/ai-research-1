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
            Play(new GreedySolver(), false);
        }

        [Test]
        public void PlayAndSave()
        {
            Play(new GreedySolver(), true);
        }

        private void Play(ISolver solver, bool saveRace)
        {
            var state = States.Generate(new Random());

            var result = Controller.PlayToEnd(state, solver, saveRace);
            
            Console.WriteLine($"Time:{result.Time} - Flags:{result.FlagsTaken}");
        }
    }
}