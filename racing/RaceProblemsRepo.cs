using System;
using System.Collections.Generic;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.racing
{
    public class RaceProblemsRepo
    {

        public static V[] Vectors(string s)
        {
            return s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(V.Parse).ToArray();
        }

        public static Disk[] Disks(string s)
        {
            return s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(Disk.ParseDisk).ToArray();
        }

        public static IEnumerable<RaceState> GenerateMany(int count, int fieldSize, int flagsCount, Random random) =>
            Enumerable.Range(0, count).Select(i => Generate(random, fieldSize, flagsCount, 3));

        public static RaceState Generate2Lines(int fieldSize, int flagsCount, int repeats)
        {
            var flags = Enumerable.Range(0, flagsCount / 2)
                .Select(i => new V(10 + 10 * i, 0))
                .Concat(Enumerable.Range(0, flagsCount / 2).Select(i => new V(10 + 10 * flagsCount / 2 - 10 * i, 20)))
                .ToList();
            var raceTrack = new RaceTrack(flags, new List<Disk>(), fieldSize * repeats * 2, flagsCount * repeats);
            return new RaceState(raceTrack, new Car(V.Zero, V.Zero, 0));
        }
        public static RaceState GenerateLine(int fieldSize, int flagsCount, int repeats)
        {
            var step = fieldSize / (flagsCount - 1);
            var flags = Enumerable.Range(0, flagsCount)
                .Select(i => new V(step * i, 0))
                .ToList();
            var raceTrack = new RaceTrack(flags, new List<Disk>(), fieldSize * repeats / 2, flagsCount * repeats);
            return new RaceState(raceTrack, new Car(V.Zero, V.Zero, 0));
        }

        private static RaceState GenerateZigZag(int fieldSize, int flagsCount, int repeats)
        {
            var xStep = fieldSize / flagsCount / 2;
            var flags = 
                from i in Enumerable.Range(0, flagsCount)
                let x = i%2 == 0 ? -fieldSize+i*xStep : fieldSize-i*xStep
                let y = (i * fieldSize * 4/5)%fieldSize - fieldSize/2
                select new V(x,y);

            var raceTrack = new RaceTrack(flags.ToList(), new List<Disk>(), 3*fieldSize * repeats, flagsCount * repeats);
            return new RaceState(raceTrack, new Car(V.Zero, V.Zero, 0));
        }

        public static RaceState Generate(Random random, int fieldSize = 100, int flagsCount = 5, int repeats = 2, int obstaclesCount = 0)
        {
            var carRadius = 2;
            var car = new Car(V.Zero, V.Zero, carRadius);
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
                where !flags.Append(car.Pos).Any(f => f.Dist2To(pos) < (radius*1.2+car.Radius).Squared())
                select new Disk(pos, radius);

            var raceTrack = new RaceTrack(flags, obstacles.Take(obstaclesCount).ToList(), fieldSize * 2 * repeats, flagsCount * repeats);
            return new RaceState(raceTrack, car);
        }

        public static IEnumerable<RaceState> GetTests2()
        {
            yield return new RaceState(
                new RaceTrack(Vectors("-100,-100 100,-100 100,100 -100,100"),
                    Disks("0,0,70 0,130,50 50,0,20 130,0,50 0,-200,80 -80,-80,20 -40,-110,20 0,-80,20 40,-110,20 80,-80,20 -80,-40,20 80,-40,20 -100,0,40 -180,20,30"), 
                    400, 8),
                Car.ParseCar("-120,-120 0,0 2"));
        }

        public static IEnumerable<RaceState> GetTests1()
        {
            yield return new RaceState(
                new RaceTrack(Vectors("-100,20 -100,0 -120,-10 -110,-40 -40,-40 -30,-20 30,-40 60,0 100,0 80,20"), 
                    Disks("-90,40,10 -120,20,10 -100,-20,10 0,-40,20 75,5,10 65,15,7"), 350, 23),
                Car.ParseCar("0,0 0,0 2"));
            yield return Generate(new Random(4465), 100, 6, 3, 20);
            yield return Generate(new Random(212121), 100, 6, 3, 20);
            yield return Generate(new Random(86787), 200, 6, 3, 20);
        }

        public static IEnumerable<RaceState> GetTests0()
        {
            yield return new RaceState(
                new RaceTrack(Vectors("-40,-20 -30,-30 -10,-40 20,-30 -50,20 10,30 50,-10"), Disks(""), 400, 25),
                Car.ParseCar("-35,-17 0,0 1"));
            yield return new RaceState(
                new RaceTrack(Vectors("-100,20 -100,0 -120,-10 -110,-40 -40,-40 -30,-20 30,-40 60,0 100,0 80,20"), Disks(""), 300, 21),
                Car.ParseCar("-93,23 0,0 2"));
            yield return Generate(new Random(67867), 50, 6, 3);
            yield return Generate(new Random(2342), 100, 6, 3);
            yield return Generate(new Random(456456), 200, 6, 3);
            yield return Generate(new Random(12123), 400, 6, 2);
            yield return Generate(new Random(34535), 400, 6, 2);
        }

        public static IEnumerable<RaceState> GetTests(bool hasObstacles)
        {
            if (hasObstacles) return GetTests1();
            return GetTests0();
        }
    }
}