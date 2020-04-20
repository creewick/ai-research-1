using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Interfaces.Commands;

namespace AI_Research_1.Solvers.HillClimbing.Mutators
{
    public abstract class RandomSegmentMutation : IMutation
    {
        private Solution result;

        public RandomSegmentMutation(Solution parentSolution
        )
        {
            var firstCarCommands = new List<Command>(parentSolution.FirstCarCommandsList);
            var secondCarCommands = new List<Command>(parentSolution.SecondCarCommandsList);
            var random = new Random();
            var segmentsCount = random.Next(2, firstCarCommands.Count);
            var segmentsCountToMutate = random.Next(1, segmentsCount);
            var movesCount = parentSolution.FirstCarCommandsList.Count();
            var segmentSize = movesCount / segmentsCount;
            for (var j = 0; j < segmentsCountToMutate; j++)
            {
                var segmentIndex = random.Next(0, segmentsCount);
                var startIndex = segmentIndex * segmentSize;
                FillSegment(startIndex, segmentSize, firstCarCommands, secondCarCommands);
            }

            result = new Solution(firstCarCommands, secondCarCommands);
        }

        public abstract void FillSegment(int startIndex, int segmentSize, List<Command> firstCarCommands,
            List<Command> secondCarCommands);

        public double Score { get; }

        public Solution GetResult()
        {
            return result;
        }
    }
}