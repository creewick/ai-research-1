using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers
{
    public class FlipRandomSegmentMutator : IMutator
    {
        private readonly int segmentsCountToMutate;
        private readonly int segmentsCount;

        public FlipRandomSegmentMutator(int segmentsCount = 10, int segmentsCountToMutate = 1)
        {
            this.segmentsCount = segmentsCount;
            this.segmentsCountToMutate = segmentsCountToMutate;
        }

        public IMutation Mutate(State state, Solution parentSolution)
        {
            return new FlipRandomSegmentMutation(parentSolution, segmentsCount, segmentsCountToMutate);
        }
    }

    public class FlipRandomSegmentMutation : IMutation
    {
        private readonly Solution result;

        public FlipRandomSegmentMutation(Solution parentSolution,  int segmentsCount,  int segmentsCountToMutate)
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
                (firstCarCommands, secondCarCommands) =
                    FlipSegment(startIndex, segmentSize, firstCarCommands, secondCarCommands);
            }

            result = new Solution(firstCarCommands, secondCarCommands);
        }

        private ( List<Command> firstCarCommands, List<Command> secondCarCommand) FlipSegment(int startIndex,
            in int segmentSize, List<Command> firstCarCommands,
            List<Command> secondCarCommands)
        {
            firstCarCommands
                = ReverseSegment(startIndex, segmentSize, firstCarCommands);
            secondCarCommands = ReverseSegment(startIndex, segmentSize, secondCarCommands);
            return (firstCarCommands, secondCarCommands);
        }

        private static List<Command> ReverseSegment(int startIndex, int segmentSize, List<Command> firstCarCommands)
        {
            return firstCarCommands.Take(startIndex)
                .Concat(firstCarCommands.Skip(startIndex).Take(segmentSize).Reverse())
                .Concat(firstCarCommands.Skip(startIndex + segmentSize)).ToList();
        }

        public double Score { get; }

        public Solution GetResult()
        {
            return result;
        }
    }
}