using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Interfaces;

namespace AI_Research_1.Logic
{
    public class Controller
    {
        public const int Timeout = 100;

        public State PlayToEnd(State state, ISolver solver, bool saveJson)
        {
            var states = new List<State> {state};

            while (!state.IsFinished)
            {
                state = state.Copy();
                var solution = solver.GetSolution(state.Copy(), Timeout);

                state.Tick(solution);
                
                states.Append(state);
            }

            return state;
        }
    }
}