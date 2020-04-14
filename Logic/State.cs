using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;
using AI_Research_1.Serialization;

namespace AI_Research_1.Logic
{
    public class State : IValuesList
    {
        public Track Track { get; }
        public Car FirstCar { get; }
        public Car SecondCar { get; }
        public int FlagsTaken { get; private set; }
        public int Time { get; private set; }
        
        public int Cooldown { get; set; }

        public State(Track track, Car firstCar, Car secondCar, int flagsTaken=0, int time=0, int cooldown=20)
        {
            Cooldown = cooldown;
            Track = track;
            FirstCar = firstCar;
            SecondCar = secondCar;
            FlagsTaken = flagsTaken;
            Time = time;
        }
        
        public State Copy() => new State(Track, FirstCar.Copy(), SecondCar.Copy(), FlagsTaken, Time, Cooldown);
        
        public V GetNextFlag() => Track.Flags[FlagsTaken % Track.Flags.Count];
        public V GetNextNextFlag() => Track.Flags[(FlagsTaken + 1) % Track.Flags.Count];

        public bool IsFinished => 
                Time >= Track.Time
             || FlagsTaken >= Track.FlagsGoal
             || !FirstCar.IsAlive
             || !SecondCar.IsAlive;

        public void Tick(Solution solution)
        {
            if (IsFinished) return;
            
            solution.FirstCarCommand.Apply(this, solution, FirstCar);
            solution.SecondCarCommand.Apply(this, solution, SecondCar);

            MoveCar(FirstCar);
            MoveCar(SecondCar);

            if (Cooldown > 0) Cooldown--;
            Time++;
        }

        private void MoveCar(Car car)
        {
            if (!car.IsAlive) return;

            var from = car.Pos;
            car.Tick();
            var to = car.Pos;

            if (CrashToObstacle(from, to, car.Radius))
                car.IsAlive = false;
            
            // TODO Flags Count
            while (GetNextFlag().SegmentCrossPoint(from, to, car.Radius))
                FlagsTaken++;
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
        
        public object[] GetValuesList() => new object[] {FlagsTaken, Cooldown, FirstCar, SecondCar};
    }
}