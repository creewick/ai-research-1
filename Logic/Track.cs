using System.Collections.Generic;
using AI_Research_1.Helpers;

namespace AI_Research_1.Logic
{
    public class Track
    {
        public readonly int Time;
        public readonly int FlagsGoal;
        public readonly IReadOnlyList<V> Flags;
        public readonly IReadOnlyList<Disk> Obstacles;

        public Track(int time, int flagsGoal, IReadOnlyList<V> flags, IReadOnlyList<Disk> obstacles)
        {
            Time = time;
            Flags = flags;
            Obstacles = obstacles;
            FlagsGoal = flagsGoal;
        }
    }
}