using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers
{
    public class RandomSegmentMutator : IMutator
    {
        private int segmentsCount;
        private int segmentsCountToMutate;

        public RandomSegmentMutator(int segmentsCount = 10, int segmentsCountToMutate = 1)
        {
            this.segmentsCount = segmentsCount;
            this.segmentsCountToMutate = segmentsCountToMutate;
        }

        public IMutation Mutate(State state, Solution parentSolution)
        {
            return new RandomSegmentMutation(parentSolution, segmentsCount, segmentsCountToMutate);
        }
    }

    public class RandomSegmentMutation : IMutation
    {
        private Solution result;

        public RandomSegmentMutation(Solution parentSolution, int segmentsCount, int segmentsCountToMutate)
        {
            var firstCarCommands = new List<Command>(parentSolution.FirstCarCommandsList);
            var secondCarCommands = new List<Command>(parentSolution.SecondCarCommandsList);
            var random = new Random();
            var movesCount = parentSolution.FirstCarCommandsList.Count();
            var segmentSize = movesCount / segmentsCount;

            for (var j = 0; j < segmentsCountToMutate; j++)
            {
                var segmentIndex = random.Next(0, segmentsCount);
                var startIndex = segmentIndex * segmentSize;
                RandomSegment(startIndex, segmentSize, firstCarCommands, secondCarCommands);
            }

            result = new Solution(firstCarCommands, secondCarCommands);
        }

        private static void RandomSegment(int startIndex, int segmentSize, List<Command> firstCarCommands,
            List<Command> secondCarCommands)
        {
            var random = new Random();
            var commands = Command.All.ToList();
            for (var i = startIndex; i < startIndex + segmentSize; i++)
            {
                var command = commands[random.Next(0, Command.All.Count())];
                firstCarCommands[i] = command;
                command = commands[random.Next(0, Command.All.Count())];
                secondCarCommands[i] = command;
            }
        }

        public double Score { get; }

        public Solution GetResult()
        {
            return result;
        }
    }
}