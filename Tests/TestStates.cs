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
                    maxCooldown: 200
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
                    flags: new[]
                    {
                        new V(0, 100), new V(-50, 50), new V(50, 50), new V(-100, 0),
                        new V(100, 0), new V(-50, -50), new V(50, -50), new V(0, -100)
                    },
                    obstacles: new List<Disk>(),
                    maxCooldown: 200
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
    }
}