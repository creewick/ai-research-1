using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;
using AI_Research_1.Solvers;
using NUnit.Framework;

namespace AI_Research_1.Tests
{
    [TestFixture]
    public class Tests
    {
        private static readonly int StatesCount = 10;
        private static readonly int FieldSize = 100;

        private readonly IEnumerable<State> noObstaclesStates =
            Enumerable.Range(0, StatesCount)
                .Select(x => States.Generate(new Random(), FieldSize, 10, 1, 0));

        private readonly IEnumerable<State> littleCountObstaclesStates =
            Enumerable.Range(0, StatesCount)
                .Select(x => States.Generate(new Random(), FieldSize, 10, 1, 2));

        private readonly IEnumerable<State> mediumCountObstaclesStates =
            Enumerable.Range(0, StatesCount)
                .Select(x => States.Generate(new Random(), FieldSize, 10, 1, 5));

        private readonly IEnumerable<State> highCountObstaclesStates =
            Enumerable.Range(0, StatesCount)
                .Select(x => States.Generate(new Random(), FieldSize, 10, 1, 10));

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

        [Test]
        public void PlayMediumCountObstacles()
        {
            mediumCountObstaclesStates.ToList().ForEach(x => Play(new GreedySolver(), x, true));
        }

        [Test]
        public void PlayLittleCountObstacles()
        {
            littleCountObstaclesStates.ToList().ForEach(x => Play(new GreedySolver(), x, true));
        }

        [Test]
        public void PlayHighCountObstacles()
        {
            highCountObstaclesStates.ToList().ForEach(x => Play(new GreedySolver(), x, true));
        }

        [Test]
        public void PlayNoObstacles()
        {
            noObstaclesStates.ToList().ForEach(x => Play(new GreedySolver(), x, true));
        }


        private void Play(ISolver solver, bool saveRace)
        {
            var state = States.Generate(new Random());

            var result = Controller.PlayToEnd(state, solver, saveRace);

            Console.WriteLine($"Time:{result.Time} - Flags:{result.FlagsTaken}");
        }

        private void Play(ISolver solver, State state, bool saveRace)
        {
            var result = Controller.PlayToEnd(state, solver, saveRace);

            Console.WriteLine($"Time:{result.Time} - Flags:{result.FlagsTaken}");
        }
    }
}