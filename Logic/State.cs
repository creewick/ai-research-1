using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;

namespace AI_Research_1.Logic
{
    public class State
    {
        public readonly Track Track;
        public readonly List<Car> Cars;
        public int FlagsTaken;
        public int Time;

        public State(Track track, List<Car> cars, int flagsTaken=0, int time=0)
        {
            Track = track;
            Cars = cars;
            FlagsTaken = flagsTaken;
            Time = time;
        }
        
        public State Copy() => new State(Track, CopyCars(), FlagsTaken, Time);
        
        public V GetNextFlag() => Track.Flags[FlagsTaken % Track.Flags.Count];

        private List<Car> CopyCars() => Cars.Select(car => car.Copy()).ToList();

        public bool IsFinished => 
                Time >= Track.Time
             || FlagsTaken >= Track.FlagsGoal
             || Cars.Any(car => !car.IsAlive);

        public void Tick(Solution solution)
        {
            if (IsFinished) return;
            
            for (var i = 0; i < Cars.Count; i++)
            {
                var car = Cars[i];
                if (!car.IsAlive) continue;

                var from = car.Pos;
                car.Tick(solution.Moves[i]);
                var to = car.Pos;

                if (CrashToObstacle(from, to, Cars[i].Radius))
                    car.IsAlive = false;

                // TODO Flags Count
                while (GetNextFlag().SegmentCrossPoint(from, to, car.Radius))
                    FlagsTaken++;
            }

            Time++;
        }

        private bool CrashToObstacle(V a, V b, int carRadius)
        {
            // optimization for speed! No linq!
            if (Track.Obstacles.Count == 0) return false;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var o in Track.Obstacles)
                if (o.Pos.SegmentCrossPoint(a, b, o.Radius + carRadius)) return true;
            return false;
        }
    }
}