using System;

namespace AiAlgorithms.Algorithms
{
    public interface IHaveTime
    {
        TimeSpan Time { get; set; }
    }
    
    public interface IHaveIndex
    {
        int MutationIndex { get; set; }
        int ImprovementIndex { get; set; }
    }
}