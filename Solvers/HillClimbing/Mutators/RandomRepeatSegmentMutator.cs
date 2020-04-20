using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.HillClimbing.Mutators
{
    public class RandomRepeatSegmentMutator : IMutator
    {
        public IMutation Mutate(State state, Solution parentSolution)
        {
            return new RandomRepeatSegmentMutation(parentSolution);
        }
    }
    public class RandomRepeatSegmentMutation : RandomSegmentMutation
    {
        public RandomRepeatSegmentMutation(Solution parentSolution) :
            base(parentSolution)
        {
        }

        public override void FillSegment(int startIndex, int segmentSize, List<Command> firstCarCommands,
            List<Command> secondCarCommands)
        {
            var random = new Random();
            var commands = Command.All.ToList();
            var firstCommand = commands[random.Next(0, Command.All.Count())];
            var secondCommand = commands[random.Next(0, Command.All.Count())];
            for (var i = startIndex; i < startIndex + segmentSize; i++)
            {
                firstCarCommands[i] = firstCommand;
                secondCarCommands[i] = secondCommand;
            }
        }
    }
}