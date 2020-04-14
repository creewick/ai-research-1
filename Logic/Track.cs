using System.Collections.Generic;
using AI_Research_1.Helpers;
using AI_Research_1.Serialization;

namespace AI_Research_1.Logic
{
    public class Track : IValuesList
    {
        public int Time { get; }
        public int FlagsGoal { get; }
        public int MaxCooldown { get; }
        public IReadOnlyList<V> Flags { get; }
        public IReadOnlyList<Disk> Obstacles { get; }

        public Track(int time, int flagsGoal, IReadOnlyList<V> flags, IReadOnlyList<Disk> obstacles, int maxCooldown)
        {
            Time = time;
            Flags = flags;
            Obstacles = obstacles;
            MaxCooldown = maxCooldown;
            FlagsGoal = flagsGoal;
        }

        public object[] GetValuesList() => new object[] {Time, FlagsGoal, MaxCooldown, Flags, Obstacles};
    }
}