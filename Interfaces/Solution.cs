using System.Collections.Generic;
using System.Linq;
using AI_Research_1.Helpers;

namespace AI_Research_1.Interfaces
{
    public class Solution
    {
        public IEnumerable<V> FirstCarMoves { get; }
        public IEnumerable<V> SecondCarMoves { get; }
        
        public Solution(IEnumerable<V> firstCarMoves, IEnumerable<V> secondCarMoves)
        {
            FirstCarMoves = firstCarMoves;
            SecondCarMoves = secondCarMoves;
        }

        public Solution NextTick() => new Solution(FirstCarMoves.Skip(1), SecondCarMoves.Skip(1));
    }
}