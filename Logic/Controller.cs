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
            var solutions = new List<IEnumerable<Solution>>();

            while (!state.IsFinished)
            {
                state = state.Copy();

                var newSolutions = GetSolutions(solver, state.Copy());
                
                state.Tick(newSolutions.Last());
                
                solutions.Add(newSolutions);
                states.Add(state);
            }

            if (saveRace) SaveRace(states, solutions);

            return state;
        }

        private static IEnumerable<Solution> GetSolutions(ISolver solver, State state)
        {
            var task = Task.Run(() => solver
                .GetSolutions(state, Timeout));
            
            task.Wait(Timeout);
            
            if (!task.IsCompleted) throw new TimeoutException();

            return task.Result;
        }

        private static void SaveRace(List<State> states, List<IEnumerable<Solution>> solutions)
        {
            var file = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "Visualization", "race.js");
            
            var visualisation = new Visualization(states, solutions, states[0].Track);
            
            var text = "const race = " + JsonSerializer.Serialize(visualisation);
            
            File.WriteAllText(file, text);
        }
    }
}