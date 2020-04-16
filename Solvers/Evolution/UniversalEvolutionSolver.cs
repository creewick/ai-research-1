using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;
using AI_Research_1.Solvers.Evolution.Appliers;
using AI_Research_1.Solvers.Evolution.Filters;
using AI_Research_1.Solvers.Evolution.Selectors;

namespace AI_Research_1.Solvers.Evolution
{
    public class UniversalEvolutionSolver : ISolver
    {
        private readonly ISolver baseSolver;
        private readonly IGeneticFilter filter;
        private readonly IGeneticApplier applier;
        private readonly IGeneticSelector selector;

        public UniversalEvolutionSolver(ISolver baseSolver, IGeneticFilter filter, IGeneticApplier applier, IGeneticSelector selector)
        {
            this.baseSolver = baseSolver;
            this.filter = filter;
            this.applier = applier;
            this.selector = selector;
        }
        
        public IEnumerable<Solution> GetSolutions(State state, Countdown time)
        {
            var population = baseSolver
                .GetSolutions(state, time / 10)
                .ToList();
            // TODO размер популяции должен быть больше  ~ 200

            while (!time.IsFinished())
            {
                var parents = filter
                    .GetParents(state, population)
                    .ToList();
                // слабо влияет

                var children = applier
                    .GetChildren(parents)
                    .ToList();
                
                // TODO elitism
                // TODO добавить случайный поиск

                population = selector
                    .GetPopulation(state, parents, children, population.Count)
                    .ToList();
            }

            return population;
        }

        public string GetNameWithArgs() =>
            $"Evolution.{baseSolver.GetNameWithArgs()};{filter.GetType().Name}.{applier.GetType().Name}.{selector.GetType().Name}";
    }
}