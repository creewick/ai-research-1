using System.Text.Json.Serialization;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;

namespace AI_Research_1.Logic
{
    public class State
    {
        [JsonIgnore] public Track Track { get; }
        public Car FirstCar { get; }
        public Car SecondCar { get; }
        public ExchangeMod ExchangeMod { get; }
        public int FlagsTaken { get; private set; }
        [JsonIgnore] public int Time { get; private set; }

        public State(Track track, Car firstCar, Car secondCar, ExchangeMod exchangeMod, int flagsTaken=0, int time=0)
        {
            Track = track;
            FirstCar = firstCar;
            SecondCar = secondCar;
            ExchangeMod = exchangeMod;
            FlagsTaken = flagsTaken;
            Time = time;
        }
        
        public State Copy() => new State(Track, FirstCar.Copy(), SecondCar.Copy(), ExchangeMod.Copy(), FlagsTaken, Time);
        
        public V GetNextFlag() => Track.Flags[FlagsTaken % Track.Flags.Count];

        public bool IsFinished => 
                Time >= Track.Time
             || FlagsTaken >= Track.FlagsGoal
             || !FirstCar.IsAlive
             || !SecondCar.IsAlive;

        public void Tick(Solution solution)
        {
            if (IsFinished) return;

            var firstExchange = solution.FirstCarCommands[0] is Exchange;
            var secondExchange = solution.SecondCarCommands[0] is Exchange;
            if (firstExchange && secondExchange && ExchangeMod.Cooldown == 0)
                ExchangeMod.Apply(FirstCar, SecondCar);
            else {
                ExchangeMod.Tick();
                MoveCar(FirstCar, firstExchange ? V.Zero : (V)solution.FirstCarCommands[0]); 
                MoveCar(SecondCar, secondExchange ? V.Zero : (V)solution.SecondCarCommands[0]);
            }
            Time++;
        }

        private void MoveCar(Car car, V move)
        {
            if (!car.IsAlive) return;

            var from = car.Pos;
            car.Tick(move);
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
    }
}