using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AI_Research_1.Interfaces;

namespace AI_Research_1.Logic
{
    public static class Controller
    {
        public const int Timeout = 100;

        public static State PlayToEnd(State state, ISolver solver, bool saveRace)
        {
            var states = new List<State> {state};

            while (!state.IsFinished)
            {
                state = state.Copy();
                
                var task = Task.Run(() => solver.GetSolutions(state.Copy(), Timeout).Last());
                task.Wait(Timeout);
                
                if (!task.IsCompleted)
                    throw new TimeoutException();
                
                state.Tick(task.Result);
                
                states.Add(state);
            }

            if (saveRace) SaveRace(states);

            return state;
        }

        private static void SaveRace(List<State> states)
        {
            var file = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "Visualization", "race.js");
            
            var visualisation = new Visualization(states, states[0].Track);
            
            var text = "const race = " + JsonSerializer.Serialize(visualisation);
            
            File.WriteAllText(file, text);
        }
    }
}