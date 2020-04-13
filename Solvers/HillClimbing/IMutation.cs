using AI_Research_1.Interfaces;

namespace AI_Research_1.Solvers
{
    public interface IMutation
    {
        double Score { get; }
        Solution GetResult();
    }
}