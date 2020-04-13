using System;
using AI_Research_1.Helpers;
using AI_Research_1.Interfaces;

namespace AI_Research_1.Logic
{
    public class Move : Command
    {
        private readonly V v;
        
        public Move(int x, int y)
        {
            if (x < -1 || x > 1 || y < -1 || y > 1)
                throw new ArgumentException();
            
            v = new V(x, y); 
        }
        
        public override void Apply(State state, Solution solution, Car car)
        {
            car.V += v;
        }
    }
}