using AI_Research_1.Helpers;

namespace AI_Research_1.Interfaces
{
    public class Solution
    {
        public V[] FirstCarMoves { get; }
        public V[] SecondCarMoves { get; }
        
        public Solution(V[] firstCarMoves, V[] secondCarMoves)
        {
            FirstCarMoves = firstCarMoves;
            SecondCarMoves = secondCarMoves;
        }
    }
}