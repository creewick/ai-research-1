using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers
{
    public class RandomSegmentMutator : IMutator
    {
        public IMutation Mutate(State state, Solution parentSolution)
        {
            return new RandomSegmentMutation(parentSolution);
        }
    }

    public class RandomSegmentMutation : IMutation
    {
        private int segmentsCount = 10;
        private Solution result;

        public RandomSegmentMutation(Solution parentSolution)
        {
            var random = new Random();
            var movesCount = parentSolution.FirstCarCommandsList.Count();
            var segmentSize = movesCount / segmentsCount;
            var segmentIndex = random.Next(0, segmentsCount);
            var startIndex = segmentIndex * segmentSize;
            var firstCarCommands = new List<Command>(parentSolution.FirstCarCommandsList);
            var secondCarCommands = new List<Command>(parentSolution.SecondCarCommandsList);
            var commands = Command.All.ToList();
            for (var i = startIndex; i < startIndex + segmentSize; i++)
            {
                var command = commands[random.Next(0, Command.All.Count())];
                firstCarCommands[i] = command;
                command = commands[random.Next(0, Command.All.Count())];
                secondCarCommands[i] = command;
            }

            result = new Solution(firstCarCommands, secondCarCommands);
        }

        public double Score { get; }

        public Solution GetResult()
        {
            return result;
        }
    }
}