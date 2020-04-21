using System.Collections.Generic;
using AI_Research_1.Helpers;
using AI_Research_1.Logic;

namespace AI_Research_1.Tests
{
    // Только public static поля через лямбду или { get; }, возвращающие State
    public class TestStates
    {
        public static State NoBlocks_Circle => 
            new State(
                track: new Track(
                    time: 400,
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
                    v: new V(0, 0),
                    radius: 2
                ),
                secondCar: new Car(
                    pos: new V(0, 0),
                    v: new V(0, 0),
                    radius: 2
                )
            );

        public static State NoBlocks_ZigZag =>
            new State(
                track: new Track(
                    time: 400,
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
                    v: new V(0, 0),
                    radius: 2
                ),
                secondCar: new Car(
                    pos: new V(0, 0),
                    v: new V(0, 0),
                    radius: 2
                )
            );

        private const int Zoom = 10;
        public static State ExchangeMap =>
            new State(
                track: new Track(
                    time: 400,
                    flagsGoal: 16,
                    flags: new[]
                    {
                        new V(-10*Zoom, 8*Zoom), new V(10*Zoom, -8*Zoom),
                        new V(-10*Zoom, -8*Zoom), new V(10*Zoom, 8*Zoom)
                    },
                    obstacles: new[]
                    {
                        new Disk(-24*Zoom, 32*Zoom, 8*Zoom), new Disk(-8*Zoom, 32*Zoom, 8*Zoom),
                        new Disk(8*Zoom, 32*Zoom, 8*Zoom), new Disk(24*Zoom, 32*Zoom, 8*Zoom),
                        
                        new Disk(-24*Zoom, 16*Zoom, 8*Zoom), new Disk(-24*Zoom, -16*Zoom, 8*Zoom),
                        new Disk(24*Zoom, 16*Zoom, 8*Zoom), new Disk(24*Zoom, -16*Zoom, 8*Zoom),
                        new Disk(-24*Zoom, 0, 8*Zoom), new Disk(24*Zoom, 0, 8*Zoom),
                        
                        new Disk(-24*Zoom, -32*Zoom, 8*Zoom), new Disk(-8*Zoom, -32*Zoom, 8*Zoom),
                        new Disk(8*Zoom, -32*Zoom, 8*Zoom), new Disk(24*Zoom, -32*Zoom, 8*Zoom),

                        new Disk(0, 24*Zoom, 4*Zoom), new Disk(0, 16*Zoom, 4*Zoom),
                        new Disk(0, 8*Zoom, 4*Zoom), new Disk(0, 0, 4*Zoom),
                        new Disk(0, -8*Zoom, 4*Zoom), new Disk(0, -16*Zoom, 4*Zoom),
                        new Disk(0, -24*Zoom, 4*Zoom),
                    },
                    maxCooldown: 20
                ),
                firstCar: new Car(
                    pos: new V(-10*Zoom, -8*Zoom),
                    v: V.Zero,
                    radius: 2
                ),
                secondCar: new Car(
                    pos: new V(10*Zoom, 8*Zoom),
                    v: V.Zero,
                    radius: 2
                )
            );

        public static State BottleNeck =>
            new State(
                track: new Track(
                    time: 100,
                    flagsGoal: 2,
                    flags: new[]{new V(0,0), new V(0, 0)},
                    obstacles:new[]
                    {
                        new Disk(0, 40, 30),
                        new Disk(-35, 0, 30),
                        new Disk(35, 0, 30)
                    },
                    maxCooldown:20
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
        public static State BottleNeck2 =>
            new State(
                track: new Track(
                    time: 200,
                    flagsGoal: 2,
                    flags: new[]{new V(0,0), new V(0, 0)},
                    obstacles:new[]
                    {
                        new Disk(0, 40, 30),
                        new Disk(-35, 0, 30),
                        new Disk(35, 0, 30)
                    },
                    maxCooldown:20
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


        public static State Snake =>
            new State(
                track: new Track(
                    time: 200,
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

        public static State Cross =>
            new State(
                track: new Track(
                    time: 100,
                    flagsGoal: 40,
                    flags: new[] {new V(0, 9), new V(0, -9), new V(9, 0), new V(-9, 0)},
                    obstacles: new[]
                    {
                        new Disk(0, 0, 6), new Disk(18, 0, 6), new Disk(-18, 0, 6),
                        new Disk(0, 18, 6), new Disk(0, -18, 6)
                    },
                    maxCooldown:20
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
}