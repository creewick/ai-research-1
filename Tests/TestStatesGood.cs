using System.Collections.Generic;
using AI_Research_1.Helpers;
using AI_Research_1.Logic;

namespace AI_Research_1.Tests
{
    // Только public static поля через лямбду или { get; }, возвращающие State
    public class TestStatesGoodNoBlocks
    {
        private const int Zoom = 10;

        public static State NoBlocksCircle =>
            new State(
                track: new Track(
                    time: 350,
                    flagsGoal: 16,
                    flags: new[]
                    {
                        new V(0, 100), new V(50, 50), new V(100, 0), new V(50, -50),
                        new V(0, -100), new V(-50, -50), new V(-100, 0), new V(-50, 50)
                    },
                    obstacles: new List<Disk>(),
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    pos: new V(0, 0),
                    v: V.Zero,
                    radius: 2
                ),
                secondCar: new Car(
                    pos: new V(0, 0),
                    v: V.Zero,
                    radius: 2
                )
            );

        public static State NoBlocksZigZag =>
            new State(
                track: new Track(
                    time: 350,
                    flagsGoal: 16,
                    new[]
                    {
                        new V(0, 100), new V(-50, 50), new V(50, 50), new V(-100, 0),
                        new V(100, 0), new V(-50, -50), new V(50, -50), new V(0, -100)
                    },
                    obstacles: new List<Disk>(),
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    pos: new V(0, 0),
                    v: V.Zero,
                    radius: 2
                ),
                secondCar: new Car(
                    pos: new V(0, 0),
                    v: V.Zero,
                    radius: 2
                )
            );

        public static State NoObstacles2Flags =>
            new State(
                track: new Track(
                    time: 200,
                    flagsGoal: 2,
                    flags: new[]
                    {
                        new V(46, 37), new V(-67, -62)
                    },
                    obstacles: new List<Disk>(),
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    V.Zero, V.Zero, 2
                ),
                secondCar: new Car(
                    new V(100, 100),
                    V.Zero, 2)
            );

        public static State NoObstacles2Flags5Repeats =>
            new State(
                track: new Track(
                    time: 150,
                    flagsGoal: 10,
                    flags: new[]
                    {
                        new V(46, 37), new V(-67, -62)
                    },
                    obstacles: new List<Disk>(),
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    V.Zero, V.Zero, 2
                ),
                secondCar: new Car(
                    new V(100, 100),
                    V.Zero, 2)
            );

        public static State NoObstacles28Flags =>
            new State(
                track: new Track(
                    time: 350,
                    flagsGoal: 28,
                    flags: new[]
                    {
                        new V(46, 37), new V(-67, -62), new V(-36, -32), new V(-21, 3), new V(-49, 14), new V(-24, -35),
                        new V(2, -88), new V(35, -74), new V(99, 13), new V(31, -65), new V(69, -23),
                        new V(-75, -80), new V(-15, 94), new V(38, -27), new V(-49, 78), new V(16, 69), new V(71, 27),
                        new V(74, 83), new V(38, 22), new V(79, -99), new V(0, 100), new V(50, 50), new V(100, 0),
                        new V(50, -50),
                        new V(0, -100), new V(-50, -50), new V(-100, 0), new V(-50, 50)
                    },
                    obstacles: new List<Disk>(),
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    V.Zero, V.Zero, 2
                ),
                secondCar: new Car(
                    new V(100, 100),
                    V.Zero, 2)
            );
    }

    public class TestStatesGoodWithBlocks
    {
        private const int Zoom = 10;

        public static State ExchangeMap =>
            new State(
                track: new Track(
                    time: 350,
                    flagsGoal: 16,
                    flags: new[]
                    {
                        new V(-10 * Zoom, 8 * Zoom), new V(10 * Zoom, -8 * Zoom),
                        new V(-10 * Zoom, -8 * Zoom), new V(10 * Zoom, 8 * Zoom)
                    },
                    obstacles: new[]
                    {
                        new Disk(-24 * Zoom, 32 * Zoom, 8 * Zoom), new Disk(-8 * Zoom, 32 * Zoom, 8 * Zoom),
                        new Disk(8 * Zoom, 32 * Zoom, 8 * Zoom), new Disk(24 * Zoom, 32 * Zoom, 8 * Zoom),

                        new Disk(-24 * Zoom, 16 * Zoom, 8 * Zoom), new Disk(-24 * Zoom, -16 * Zoom, 8 * Zoom),
                        new Disk(24 * Zoom, 16 * Zoom, 8 * Zoom), new Disk(24 * Zoom, -16 * Zoom, 8 * Zoom),
                        new Disk(-24 * Zoom, 0, 8 * Zoom), new Disk(24 * Zoom, 0, 8 * Zoom),

                        new Disk(-24 * Zoom, -32 * Zoom, 8 * Zoom), new Disk(-8 * Zoom, -32 * Zoom, 8 * Zoom),
                        new Disk(8 * Zoom, -32 * Zoom, 8 * Zoom), new Disk(24 * Zoom, -32 * Zoom, 8 * Zoom),

                        new Disk(0, 24 * Zoom, 4 * Zoom), new Disk(0, 16 * Zoom, 4 * Zoom),
                        new Disk(0, 8 * Zoom, 4 * Zoom), new Disk(0, 0, 4 * Zoom),
                        new Disk(0, -8 * Zoom, 4 * Zoom), new Disk(0, -16 * Zoom, 4 * Zoom),
                        new Disk(0, -24 * Zoom, 4 * Zoom),
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    pos: new V(-10 * Zoom, -8 * Zoom),
                    v: V.Zero,
                    radius: 2
                ),
                secondCar: new Car(
                    pos: new V(10 * Zoom, 8 * Zoom),
                    v: V.Zero,
                    radius: 2
                )
            );


        public static State With5Obstacles5Flags2Repeats =>
            new State(
                track: new Track(
                    time: 350,
                    flagsGoal: 10,
                    flags: new[]
                    {
                        new V(-49, -0), new V(-26, -88), new V(-53, 39), new V(-95, 49), new V(63, 64)
                    },
                    obstacles: new[]
                    {
                        new Disk(-49, 76, 8), new Disk(-5, -31, 7), new Disk(-72, 97, 7),
                        new Disk(24, 78, 6), new Disk(-8, -42, 5)
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    V.Zero, V.Zero, 2
                ),
                secondCar: new Car(
                    new V(100, 100),
                    V.Zero, 2)
            );


        public static State With3Obstacles3Flags =>
            new State(
                track: new Track(
                    time: 350,
                    flagsGoal: 3,
                    flags: new[]
                    {
                        new V(-36, -32), new V(-21, 3), new V(-49, 14)
                    },
                    obstacles: new[]
                    {
                        new Disk(29, 29, 5), new Disk(-15, -15, 5), new Disk(17, 17, 5)
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    V.Zero, V.Zero, 2
                ),
                secondCar: new Car(
                    new V(100, 100),
                    V.Zero, 2)
            );

        public static State With3Obstacles3Flags3Repeats =>
            new State(
                track: new Track(
                    time: 125,
                    flagsGoal: 9,
                    flags: new[]
                    {
                        new V(-36, -32), new V(-21, 3), new V(-49, 14)
                    },
                    obstacles: new[]
                    {
                        new Disk(29, 29, 5), new Disk(-15, -15, 5), new Disk(17, 17, 5)
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    V.Zero, V.Zero, 2
                ),
                secondCar: new Car(
                    new V(100, 100),
                    V.Zero, 2)
            );

        public static State With3Obstacles3Flags10Repeats =>
            new State(
                track: new Track(
                    time: 350,
                    flagsGoal: 30,
                    flags: new[]
                    {
                        new V(-36, -32), new V(-21, 3), new V(-49, 14)
                    },
                    obstacles: new[]
                    {
                        new Disk(29, 29, 5), new Disk(-15, -15, 5), new Disk(17, 17, 5)
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    V.Zero, V.Zero, 2
                ),
                secondCar: new Car(
                    new V(100, 100),
                    V.Zero, 2)
            );
    }

    public class SnakeGroup
    {
        private const int Zoom = 10;

        public static State Snake =>
            new State(
                track: new Track(
                    time: 350,
                    flagsGoal: 32,
                    flags: new[]
                    {
                        new V(-40, -40), new V(40, 40), new V(0, -20), new V(0, 20),
                        new V(40, -40), new V(-40, 40), new V(60, 0), new V(-60, 0),
                    },
                    obstacles: new[]
                    {
                        new Disk(-40, 20, 10), new Disk(-40, 0, 10), new Disk(-40, -20, 10),
                        new Disk(-20, 0, 10), new Disk(0, 0, 10), new Disk(20, 0, 10),
                        new Disk(40, 20, 10), new Disk(40, 0, 10), new Disk(40, -20, 10),
                        new Disk(0, 40, 10), new Disk(0, -40, 10),
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    pos: new V(-60, 0),
                    v: V.Zero,
                    radius: 2
                ),
                secondCar: new Car(
                    pos: new V(60, 0),
                    v: V.Zero,
                    radius: 2
                )
            );
    }

    public class BottleNeckGroup
    {
        private const int Zoom = 10;

        public static State BottleNeck =>
            new State(
                track: new Track(
                    time: 350,
                    flagsGoal: 2,
                    flags: new[] {new V(0, 0), new V(0, 0)},
                    obstacles: new[]
                    {
                        new Disk(0, 40, 30),
                        new Disk(-35, 0, 30),
                        new Disk(35, 0, 30)
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    pos: new V(90, 0),
                    v: V.Zero,
                    radius: 2
                ),
                secondCar: new Car(
                    pos: new V(-90, 0),
                    v: V.Zero,
                    radius: 2
                )
            );
    }

    public class CrossGroup
    {
        private const int Zoom = 10;

        public static State Cross =>
            new State(
                track: new Track(
                    time: 200,
                    flagsGoal: 40,
                    flags: new[] {new V(0, 9), new V(0, -9), new V(9, 0), new V(-9, 0)},
                    obstacles: new[]
                    {
                        new Disk(0, 0, 6), new Disk(18, 0, 6), new Disk(-18, 0, 6),
                        new Disk(0, 18, 6), new Disk(0, -18, 6)
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    pos: new V(0, 9),
                    v: V.Zero,
                    radius: 2
                ),
                secondCar: new Car(
                    pos: new V(0, -9),
                    v: V.Zero,
                    radius: 2
                )
            );
    }

    public class Group10_10_3
    {
        private const int Zoom = 10;

        public static State With10Obstacles10Flags3Repeats =>
            new State(
                track: new Track(
                    time: 400,
                    flagsGoal: 30,
                    flags: new[]
                    {
                        new V(-24, -35), new V(2, -88), new V(35, -74), new V(99, 13), new V(31, -65), new V(69, -23),
                        new V(-75, -80), new V(-15, 94), new V(38, -27), new V(-49, 78), new V(16, 69), new V(71, 27),
                        new V(74, 83), new V(38, 22), new V(79, -99)
                    },
                    obstacles: new[]
                    {
                        new Disk(-44, 31, 8), new Disk(84, -39, 7), new Disk(-71, -2, 7),
                        new Disk(54, -88, 6), new Disk(22, -12, 5), new Disk(71, 47, 4), new Disk(20, 54, 8),
                        new Disk(99, -99, 10), new Disk(-79, -39, 10),
                        new Disk(51, -94, 10)
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    V.Zero, V.Zero, 2
                ),
                secondCar: new Car(
                    new V(100, 100),
                    V.Zero, 2)
            );
    }

    public class Group_5_10_0
    {
        private const int Zoom = 10;

        public static State With5Obstacles10Flags =>
            new State(
                track: new Track(
                    time: 350,
                    flagsGoal: 10,
                    flags: new[]
                    {
                        new V(69, -23),
                        new V(-75, -80), new V(-15, 94), new V(38, -27), new V(-49, 78), new V(16, 69), new V(71, 27),
                        new V(74, 83), new V(38, 22), new V(79, -99)
                    },
                    obstacles: new[]
                    {
                        new Disk(71, 47, 4), new Disk(20, 54, 8),
                        new Disk(99, -99, 10), new Disk(-79, -39, 10),
                        new Disk(51, -94, 10)
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    V.Zero, V.Zero, 2
                ),
                secondCar: new Car(
                    new V(100, 100),
                    V.Zero, 2)
            );
    }


    public class Group_7_10_0
    {
        private const int Zoom = 10;

        public static State With7Obstacles10Flags =>
            new State(
                track: new Track(
                    time: 150,
                    flagsGoal: 10,
                    flags: new[]
                    {
                        new V(-24, -35), new V(2, -88), new V(35, -74), new V(99, 13), new V(31, -65), new V(69, -23),
                        new V(-75, -80), new V(-15, 94), new V(38, -27), new V(-49, 78)
                    },
                    obstacles: new[]
                    {
                        new Disk(-44, 31, 8), new Disk(84, -39, 7), new Disk(-71, -2, 7),
                        new Disk(54, -88, 6), new Disk(22, -12, 5), new Disk(71, 47, 4), new Disk(20, 54, 8)
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    V.Zero, V.Zero, 2
                ),
                secondCar: new Car(
                    new V(100, 100),
                    V.Zero, 2)
            );
    }

    public class BottleNeck2Group
    {
        private const int Zoom = 10;

        public static State BottleNeck2 =>
            new State(
                track: new Track(
                    time: 200,
                    flagsGoal: 2,
                    flags: new[] {new V(0, 0), new V(0, 0)},
                    obstacles: new[]
                    {
                        new Disk(0, 40, 30),
                        new Disk(-35, 0, 30),
                        new Disk(35, 0, 30)
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    pos: new V(90, 100),
                    v: V.Zero,
                    radius: 2
                ),
                secondCar: new Car(
                    pos: new V(-90, 100),
                    v: V.Zero,
                    radius: 2
                )
            );
    }

    public class BottleNeck3Group
    {
        private const int Zoom = 10;

        public static State BottleNeck3 =>
            new State(
                track: new Track(
                    time: 400,
                    flagsGoal: 3,
                    flags: new[] {new V(0, 0), new V(0, 0), new V(0, -200)},
                    obstacles: new[]
                    {
                        new Disk(0, 40, 30),
                        new Disk(-35, 0, 30),
                        new Disk(35, 0, 30)
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    pos: new V(30, 100),
                    v: V.Zero,
                    radius: 2
                ),
                secondCar: new Car(
                    pos: new V(-30, 100),
                    v: V.Zero,
                    radius: 2
                )
            );
    }

    public class SprintGroup
    {
        private const int Zoom = 10;

        public static State Sprint =>
            new State(
                track: new Track(
                    time: 450,
                    flagsGoal: 16,
                    flags: new[]
                    {
                        new V(-100, 0), new V(100, 0), new V(-50, 0), new V(50, 0)
                    },
                    obstacles: new[]
                    {
                        new Disk(-125, 0, 20), new Disk(-75, 0, 20), new Disk(-25, 0, 20), new Disk(25, 0, 20), new Disk(75, 0, 20), new Disk(125, 0, 20),
                     
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    pos: new V(0, 0),
                    v: V.Zero,
                    radius: 2
                ),
                secondCar: new Car(
                    pos: new V(0, 0),
                    v: V.Zero,
                    radius: 2
                )
            );
    }
    public class Sprint2Group
    {
        private const int Zoom = 10;

        public static State Sprint2 =>
            new State(
                track: new Track(
                    time: 450,
                    flagsGoal: 16,
                    flags: new[]
                    {
                        new V(-100, 0), new V(100, 0), new V(-50, 0), new V(50, 0)
                    },
                    obstacles: new[]
                    {
                        new Disk(-125, 0, 20), new Disk(-75, 0, 20), new Disk(-25, 0, 20), new Disk(25, 0, 20), new Disk(75, 0, 20), new Disk(125, 0, 20),
                     
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    pos: new V(0, 50),
                    v: V.Zero,
                    radius: 2
                ),
                secondCar: new Car(
                    pos: new V(0, -50),
                    v: V.Zero,
                    radius: 2
                )
            );
    }
}