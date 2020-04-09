using AiAlgorithms.Algorithms;

namespace AiAlgorithms.racing
{
    public class RaceState
    {
        public readonly RaceTrack Track;
        public readonly Car Car;

        public RaceState(RaceTrack track, Car car)
        {
            Track = track;
            Car = car;
        }

        public int Time { get; private set; }
        public bool IsFinished => Time >= Track.RaceDuration || Car.FlagsTaken >= Track.FlagsToTake || !Car.IsAlive;

        public RaceState MakeCopy()
        {
            return new RaceState(Track, Car.MakeCopy()) { Time = Time };
        }

        public void Tick()
        {
            if (IsFinished) return;
            if (Car.IsAlive)
            {
                var initialPos = Car.Pos;
                Car.Tick();
                var finalPos = Car.Pos;
                if (CrashToObstacle(initialPos, finalPos, Car.Radius))
                    Car.IsAlive = false;
                else
                    while (SegmentCrossPoint(initialPos, finalPos, GetFlagFor(Car), Car.Radius))
                        Car.FlagsTaken++;
            }

            Time++;
        }

        private bool CrashToObstacle(V a, V b, int carRadius)
        {
            // optimization for speed! No linq!
            if (Track.Obstacles.Count == 0) return false;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var o in Track.Obstacles)
            {
                if (SegmentCrossPoint(a, b, o.Pos, o.Radius + carRadius)) return true;
            }
            return false;
        }

        private bool SegmentCrossPoint(V a, V b, V point, int crossDistance)
        {
            return Dist2PointToSegment(point, a, b) <= crossDistance * crossDistance;
        }

        private double Dist2PointToSegment(V p, V a, V b)
        {
            // optimized for speed! No intermediate V objects. No sqrt calls
            var bax = b.X - a.X;
            var pax = p.X - a.X;
            var pbx = p.X - b.X;
            var bay = b.Y - a.Y;
            var pay = p.Y - a.Y;
            var pby = p.Y - b.Y;
            var cos1 = bax * pax + bay * pay;
            var cos2 = -bax * pbx - bay * pby;
            if (cos1 <= 0) return p.Dist2To(a);
            if (cos2 <= 0) return p.Dist2To(b);
            double vp = bax * pay - bay * pax;
            return vp * vp / a.Dist2To(b);
        }

        public V GetFlagFor(Car car)
        {
            return Track.Flags[car.FlagsTaken % Track.Flags.Count];
        }

        public override string ToString()
        {
            return $"Car: {Car}, Time: {Time}, IsFinished: {IsFinished}";
        }
    }
}