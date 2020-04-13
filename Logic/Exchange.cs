using AI_Research_1.Interfaces;

namespace AI_Research_1.Logic
{
    public class Exchange : Command
    {
        public override void Apply(State state, Solution solution, Car car)
        {
            if (this == solution.FirstCarCommand &&
                solution.FirstCarCommand is Exchange && 
                solution.SecondCarCommand is Exchange)
            {
                (state.FirstCar.V, state.SecondCar.V) = (state.SecondCar.V, state.FirstCar.V);
            }
        }
    }
}