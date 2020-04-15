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
            new GreedySolver(10), 
            new FilterHalf(), 
            new SegmentCrossingOver(), 
            new SelectTopScore()
        );
        
        public IEnumerable<Solution> GetSolutions(State state, Countdown time) => solver.GetSolutions(state, time);
    }
}