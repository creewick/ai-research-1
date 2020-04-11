using System.Collections.Generic;
using AI_Research_1.Helpers;
using AI_Research_1.Logic;

namespace AI_Research_1.Interfaces
{
    public interface ISolver
    {
        Solution GetSolution(State state, Countdown time);
    }
}