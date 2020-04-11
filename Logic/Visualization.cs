using System.Collections.Generic;
using AI_Research_1.Interfaces;

namespace AI_Research_1.Logic
{
    public class Visualization
    {
        public Track Track { get; }
        public List<State> States { get; }
        public List<IEnumerable<Solution>> Solutions { get; }
        public Visualization(List<State> states, List<IEnumerable<Solution>> solutions, Track track)
        {
            Solutions = solutions;
            States = states;
            Track = track;
        }
    }
}