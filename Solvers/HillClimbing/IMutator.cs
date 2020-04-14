using AI_Research_1.Interfaces;
using AI_Research_1.Logic;

namespace AI_Research_1.Solvers
{
    public interface IMutator
    {
        IMutation Mutate(State state, Solution parentSolution);
    }
}