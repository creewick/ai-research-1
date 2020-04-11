using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AI_Research_1.Interfaces;

namespace AI_Research_1.Logic
{
    public class Controller
    {
        public const int Timeout = 100;

        public static State PlayToEnd(State state, ISolver solver, bool saveJson)
        {
            var states = new List<State> {state};

            while (!state.IsFinished)
            {
                state = state.Copy();
                
                var task = Task.Run(() => solver.GetSolution(state.Copy(), Timeout));
                task.Wait(Timeout);
                
                if (!task.IsCompleted)
                    throw new TimeoutException();
                
                state.Tick(task.Result);
                
                states.Add(state);
            }

            return state;
        }
    }
}