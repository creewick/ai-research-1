using System;
using System.Collections.Generic;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.Evolution
{
    public class RandomSearch : ISolver
    {
        private readonly int populationCount;
        private readonly int solutionDepth;
        private readonly Random random;

        public RandomSearch(int populationCount, int solutionDepth)
        {
            this.populationCount = populationCount;
            this.solutionDepth = solutionDepth;
            this.random = new Random();
        }
        
        public IEnumerable<Solution> GetSolutions(State state, Countdown time)
        {
            for (var i = 0; i < populationCount; i++)
            {
                
            }
        }
    }
}