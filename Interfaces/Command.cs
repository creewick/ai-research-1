using System;
using System.Collections.Generic;
using AI_Research_1.Interfaces.Commands;
using AI_Research_1.Logic;
using AI_Research_1.Serialization;

namespace AI_Research_1.Interfaces
{
    public abstract class Command : IValuesList
    {
        public abstract void Apply(State state, Solution solution, Car car);
        
        public abstract object[] GetValuesList();

        public static List<Command> All => new List<Command>
        {
            new Move(-1, 1), new Move(0, 1), new Move(1, 1),
            new Move(-1, 0), new Move(0, 0), new Move(1, 0),
            new Move(-1, -1), new Move(0, -1), new Move(1, -1),
            new Exchange()
        };

        public static Command Random(Random random) => All[random.Next(0, All.Count)];
    }
}