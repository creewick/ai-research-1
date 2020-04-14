using AI_Research_1.Logic;

namespace AI_Research_1.Interfaces.Commands
{
    public class Exchange : Command
    {
        public override void Apply(State state, Solution solution, Car car)
        {
            if (state.Cooldown == 0 &&
                this == solution.FirstCarCommand &&
                solution.FirstCarCommand is Exchange && 
                solution.SecondCarCommand is Exchange)
            {
                state.Cooldown = state.Track.MaxCooldown;
                (state.FirstCar.V, state.SecondCar.V) = (state.SecondCar.V, state.FirstCar.V);
            }
        }

        public override object[] GetValuesList() => new[] {"E"};
    }
}