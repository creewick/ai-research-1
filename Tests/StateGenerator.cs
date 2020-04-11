using System;
using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Helpers;
using AI_Research_1.Logic;

namespace AI_Research_1.Tests
{
    public class StateGenerator
    {
        public static State Generate(Random random, int fieldSize = 100, int flagsCount = 5, int repeats = 2, int obstaclesCount = 0)
        {
            var carRadius = 2;
            var car1 = new Car(V.Zero, V.Zero, carRadius);
            var car2 = new Car(new V(100, 100), V.Zero, carRadius);
            var flags = new List<V>();
            
            while(flags.Count < flagsCount)
            {
                var f = new V(random.Next(-fieldSize, fieldSize), random.Next(-fieldSize, fieldSize));
                if (flags.Any(old => old.Dist2To(f) < carRadius * carRadius)) continue;
                flags.Add(f);
            }
            var obstacles =
                from i in Enumerable.Range(0, int.MaxValue)
                let pos = new V(random.Next(-fieldSize, fieldSize), random.Next(-fieldSize, fieldSize))
                let radius = fieldSize / 10
                where !flags.Append(car1.Pos).Append(car2.Pos).Any(f => f.Dist2To(pos) < Math.Pow(radius*1.2+carRadius, 2))
                select new Disk(pos, radius);

            var track = new Track(fieldSize * 2 * repeats, flagsCount * repeats, flags, obstacles.Take(obstaclesCount).ToList());
            return new State(track, car1, car2);
        }

    }
}