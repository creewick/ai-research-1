using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Interfaces.Commands;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.HillClimbing.Mutators
{
    public class RandomAndDoNothingSegmentMutator : IMutator
    {
        public IMutation Mutate(State state, Solution parentSolution)
        {
            return new RandomAndDoNothingSegmentMutation(parentSolution);
        }
    }

    public class RandomAndDoNothingSegmentMutation : RandomSegmentMutation
    {
        public RandomAndDoNothingSegmentMutation(Solution parentSolution)
            : base(parentSolution)
        {
        }

        public override void FillSegment(int startIndex, int segmentSize, List<Command> firstCarCommands,
            List<Command> secondCarCommands)
        {
            var random = new Random();
            var commands = Command.All.ToList();
            var firstCommand = commands[random.Next(0, Command.All.Count())];
            var secondCommand = commands[random.Next(0, Command.All.Count())];
            firstCarCommands[startIndex] = firstCommand;
            secondCarCommands[startIndex] = secondCommand;
            for (var i = startIndex + 1; i < startIndex + segmentSize; i++)
            {
                firstCarCommands[i] = new Move(0, 0);
                secondCarCommands[i] = new Move(0, 0);
            }
        }
    }
}