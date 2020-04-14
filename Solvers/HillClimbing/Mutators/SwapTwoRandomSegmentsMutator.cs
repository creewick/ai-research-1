﻿using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers
{
    public class SwapTwoRandomSegmentsMutator : IMutator
    {
        private int segmentsCount;

        public SwapTwoRandomSegmentsMutator(int segmentsCount = 10)
        {
            this.segmentsCount = segmentsCount;
        }

        public IMutation Mutate(State state, Solution parentSolution)
        {
            return new SwapTwoRandomSegmentsMutation(parentSolution, segmentsCount);
        }
    }

    public class SwapTwoRandomSegmentsMutation : IMutation
    {
        private Solution result;

        public SwapTwoRandomSegmentsMutation(Solution parentSolution, int segmentsCount)
        {
            var firstCarCommands = new List<Command>(parentSolution.FirstCarCommandsList);
            var secondCarCommands = new List<Command>(parentSolution.SecondCarCommandsList);
            var random = new Random();
            var movesCount = parentSolution.FirstCarCommandsList.Count();
            var segmentSize = movesCount / segmentsCount;
            var segmentIndex = random.Next(0, segmentsCount - 1);
            var startIndex = segmentIndex * segmentSize;
            firstCarCommands =
                SwapSegments(startIndex, segmentSize, firstCarCommands);
            secondCarCommands =
                SwapSegments(startIndex, segmentSize, secondCarCommands);
            result = new Solution(firstCarCommands, secondCarCommands);
        }

        private List<Command> SwapSegments(int startIndex,
            int segmentSize, List<Command> carCommands)
        {
            for (var i = startIndex; i < startIndex + segmentSize; i++)
            {
                var firstSegmentCarCommand = carCommands[i];
                carCommands[i] = carCommands[i + segmentSize];
                carCommands[i + segmentSize] = firstSegmentCarCommand;
            }

            return carCommands;
        }

        public double Score { get; }

        public Solution GetResult()
        {
            return result;
        }
    }
}