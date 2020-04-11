using System.Collections.Generic;

namespace AI_Research_1.Logic
{
    public class Visualization
    {
        public Track Track { get; }
        public List<State> States { get; }
        public Visualization(List<State> states, Track track)
        {
            States = states;
            Track = track;
        }
    }
}