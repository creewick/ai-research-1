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
                    time: 400,
                    flagsGoal: 2,
                    flags: new[]{new V(0,0), new V(0, 0)},
                    obstacles:new[]
                    {
                        new Disk(0, 70, 60),
                        new Disk(-70, -20, 60),
                        new Disk(70, -20, 60)
                    },
                    maxCooldown:20
                ),
                firstCar: new Car(
                    pos: new V(150, -20),
                    v: V.Zero,
                    radius: 2
                ),
                secondCar: new Car(
                    pos: new V(-150, -20),
                    v: V.Zero,
                    radius: 2
                )
            );
    }
}