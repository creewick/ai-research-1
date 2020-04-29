using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers.Evolution.BaseSolvers
{
    public class RandomSolver
    {
        private readonly int solutionDepth;
        private readonly Random random;

        public RandomSolver(int solutionDepth)
        {
            this.solutionDepth = solutionDepth;
            random = new Random();
        }

        public string GetNameWithArgs() => $"Random.{solutionDepth}";
        
        public IEnumerable<Solution> GetSolutions(State state, int size)
        {
            for (var i = 0; i < size; i++)
            {
                var firstCarMoves = new List<Command>();
                var secondCarMoves = new List<Command>();
                
                for (var j = 0; j < solutionDepth; j++)
                {
                    firstCarMoves.Add(Command.Random(random));
                    secondCarMoves.Add(Command.Random(random));
                }
                
                yield return new Solution(firstCarMoves, secondCarMoves);
            }
        }
    }
}