using System.Collections.Generic;
using AI_Research_1.Logic;

namespace AI_Research_1.Interfaces
{
    public abstract class Command
    {
        public abstract void Apply(State state, Solution solution, Car car);
        
        public static List<Command> All => new List<Command>
        {
            new Move(-1, 1), new Move(0, 1), new Move(1, 1),
            new Move(-1, 0), new Move(0, 0), new Move(1, 0),
            new Move(-1, -1), new Move(0, -1), new Move(1, -1),
            new Exchange()
        };
    }
}