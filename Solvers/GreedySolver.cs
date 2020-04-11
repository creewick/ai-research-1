using System.Collections.Generic;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers
{
    public class GreedySolver : ISolver
    {
        private static readonly ISolver Solver =
            new UniversalGreedySolver(17, SimulateBy.Repeat, AggregateBy.Max);

        public IEnumerable<Solution> GetSolutions(State state, Countdown time) => Solver.GetSolutions(state, time);
    }

    public class UniversalGreedySolver : ISolver
    {
        private readonly int steps;
        private readonly SimulateBy simulate;
        private readonly AggregateBy aggregate;
        
        public UniversalGreedySolver(int steps, SimulateBy simulate, AggregateBy aggregate)
        {
            this.steps = steps;
            this.simulate = simulate;
            this.aggregate = aggregate;
        }
        
        public IEnumerable<Solution> GetSolutions(State state, Countdown time)
        {
            V? bestMove = null;
            double bestScore = double.MinValue;

            foreach (var move in PossibleMoves)
            {
                var score = Emulate(state, move);
                if (score > bestScore)
                {
                    bestMove = move;
                    bestScore = score;
                }
            }

            yield return new Solution(new[] {bestMove.Value});
        }
        
        private double Emulate(State state, V move)
        {
            var copy = state.Copy();
            var stepsLeft = steps;
            var score = double.MinValue;

            while (stepsLeft > 0)
            {
                if (steps == stepsLeft || simulate == SimulateBy.Repeat)
                    copy.Tick(new Solution(new[]{move}));

                var newScore = GetScore(copy);
                
                if (aggregate == AggregateBy.Last)
                    score = newScore;
                else if (aggregate == AggregateBy.Max && newScore > score)
                    score = newScore;
                
                stepsLeft--;
            }

            return score;
        }

        private static double GetScore(State state) =>
            + 1000000 * state.FlagsTaken
            - state.GetNextFlag().Dist2To(state.Cars[0].Pos);

        private static List<V> PossibleMoves => new List<V>
        {
            new V(-1, 1), new V(0, 1), new V(1, 1),
            new V(-1, 0), new V(0, 0), new V(1, 0),
            new V(-1, -1), new V(0, -1), new V(1, -1)
        };
    }
    
    public enum SimulateBy
    {
        DoNothing,
        Repeat
    }

    public enum AggregateBy
    {
        Max,
        Last
    }
}