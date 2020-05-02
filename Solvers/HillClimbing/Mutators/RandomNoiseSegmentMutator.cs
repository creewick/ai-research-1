using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.HillClimbing.Mutators
{
    public class RandomNoiseSegmentMutator : IMutator
    {
        public IMutation Mutate(State state, Solution parentSolution)
        {
            return new RandomNoiseSegmentMutation(parentSolution);
        }
    }
    public class RandomNoiseSegmentMutation : RandomSegmentMutation
    {
        public RandomNoiseSegmentMutation(Solution parentSolution) : base(
            parentSolution)
        {
        }

        public override void FillSegment(int startIndex, int segmentSize, List<Command> firstCarCommands,
            List<Command> secondCarCommands, int carToMutate)
        {
            List<Command> carCommands = carToMutate == 1 ? firstCarCommands : secondCarCommands;
            var random = new Random();
            var commands = Command.All.ToList();
            for (var i = startIndex; i < startIndex + segmentSize; i++)
            {
                var command = commands[random.Next(0, Command.All.Count())];
                carCommands[i] = command;
//               command = commands[random.Next(0, Command.All.Count())];
//                secondCarCommands[i] = command;
            }
        }
    }
}