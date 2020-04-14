using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AI_Research_1.Interfaces;
using AI_Research_1.Serialization;

namespace AI_Research_1.Logic
{
    public static class Controller
    {
        public const int Timeout = 100;

        public static State PlayToEnd(State state, ISolver solver, string saveFile=null)
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

            if (saveFile != null) SaveRace(states, solutions, saveFile);

            return state;
        }

        private static IEnumerable<Solution> GetSolutions(ISolver solver, State state)
        {
            var task = Task.Run(() => solver
                .GetSolutions(state, Timeout));
            
            task.Wait(Timeout);
            
            // if (!task.IsCompleted) throw new TimeoutException();

            return task.Result;
        }

        private static void SaveRace(List<State> states, List<IEnumerable<Solution>> solutions, string saveFile)
        {
            var file = Path.Combine(
                Environment.CurrentDirectory, 
                "..", "..", "..", 
                "Visualization", 
                saveFile + ".js");

            var text = "let data=" + Serializer.Serialize(new object[]{states[0].Track, states, solutions});
             
            File.WriteAllText(file, text);
        }
    }
}