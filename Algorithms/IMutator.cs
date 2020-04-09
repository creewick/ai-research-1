namespace AiAlgorithms.Algorithms
{
    public interface IMutator<in TProblem, TSolution>
    {
        IMutation<TSolution> Mutate(TProblem problem, TSolution parentSolution);
    }
}