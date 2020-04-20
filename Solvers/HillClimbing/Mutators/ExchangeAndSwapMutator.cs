using System;
using System.Collections.Generic;
using AI_Research_1.Interfaces;
using AI_Research_1.Interfaces.Commands;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.HillClimbing.Mutators
{
    public class ExchangeAndSwapMutator : IMutator
    {
        public IMutation Mutate(State state, Solution parentSolution)
        {
            return new ExchangeAndSwapMutation(parentSolution);
        }
    }

    public class ExchangeAndSwapMutation : IMutation
    {
        private Solution result;

        public ExchangeAndSwapMutation(Solution parentSolution)
        {
            var firstCarCommands = new List<Command>(parentSolution.FirstCarCommandsList);
            var secondCarCommands = new List<Command>(parentSolution.SecondCarCommandsList);
            firstCarCommands[0] = new Exchange();
            secondCarCommands[1] = new Exchange();
            result = new Solution(secondCarCommands, firstCarCommands);
        }

        public double Score { get; }

        public Solution GetResult()
        {
            return result;
        }
    }
}