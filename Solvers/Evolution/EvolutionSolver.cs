using System.Collections.Generic;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;
using AI_Research_1.Solvers.Evolution.Appliers;
using AI_Research_1.Solvers.Evolution.BaseSolvers;
using AI_Research_1.Solvers.Evolution.Filters;
using AI_Research_1.Solvers.Evolution.Selectors;

namespace AI_Research_1.Solvers.Evolution
{
    public class EvolutionSolver : ISolver
    {
        private readonly ISolver solver = new UniversalEvolutionSolver(
            new CombinedSolver(
                new ISolver[]
                {
                    new GreedySolver(10),
                    new BaseSolvers.RandomSolver(10)   
                }, 
                200
            ),
            new NormalizeFilter(), 
            new RandomNoiseSegment(), 
            new SelectTopScore()
        );

        public string GetNameWithArgs() => solver.GetNameWithArgs();

        public IEnumerable<Solution> GetSolutions(State state, Countdown time) => solver.GetSolutions(state, time);
    }
}