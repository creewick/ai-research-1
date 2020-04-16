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
        private static readonly string ProjectDirectory = Path.Combine(Environment.CurrentDirectory, "..", "..", "..");
        private const int Timeout = 100;

        public static State PlayToEnd(State state, ISolver solver, string replayFile=null, string statsFile=null)
        {
            var states = new List<State> {state};
            var solutions = new List<IEnumerable<Solution>>();

            while (!state.IsFinished)
            {
                state = state.Copy();

                var newSolutions = GetSolutions(solver, state.Copy()).ToList();
                
                state.Tick(newSolutions.Last());
                
                solutions.Add(newSolutions.TakeLast(20));
                states.Add(state);
            }

            if (replayFile != null) SaveReplay(states, solutions, replayFile);
            if (statsFile != null) SaveStats(state, statsFile);

            return state;
        }

        private static IEnumerable<Solution> GetSolutions(ISolver solver, State state)
        {
            var task = Task.Run(() => solver
                .GetSolutions(state, Timeout));
            
            task.Wait(Timeout);

            return task.Result;
        }

        private static void SaveReplay(List<State> states, List<IEnumerable<Solution>> solutions, string replayFile)
        {
            var file = Path.Combine(ProjectDirectory, "Visualization", replayFile);

            var text = "let data=" + Serializer.Serialize(new object[]{states[0].Track, states, solutions});
            
            File.WriteAllText(file, text);
        }

        private static void SaveStats(State result, string statsFile)
        {
            var file = Path.Combine(ProjectDirectory, "Statistics", statsFile);
            var track = result.Track;
            var text = $"{track.FlagsGoal},{track.Time},{result.FlagsTaken},{result.Time}\n";
            
            File.AppendAllText(file, text);
        }
    }
}