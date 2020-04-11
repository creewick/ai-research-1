using AI_Research_1.Helpers;

namespace AI_Research_1.Interfaces
{
    public class Solution
    {
        public readonly V[] FirstCarMoves;
        public readonly V[] SecondCarMoves;
        
        public Solution(V[] firstCarMoves, V[] secondCarMoves)
        {
            FirstCarMoves = firstCarMoves;
            SecondCarMoves = secondCarMoves;
        }
    }
}