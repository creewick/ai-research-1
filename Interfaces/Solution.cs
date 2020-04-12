using System.Collections.Generic;
using System.Text.Json.Serialization;
using AI_Research_1.Helpers;

namespace AI_Research_1.Interfaces
{
    public class Solution
    {
        [JsonPropertyName("Car1")] public IEnumerable<V> FirstCarMoves { get; }
        [JsonPropertyName("Car2")] public IEnumerable<V> SecondCarMoves { get; }
        
        public Solution(IEnumerable<V> firstCarMoves, IEnumerable<V> secondCarMoves)
        {
            FirstCarMoves = firstCarMoves;
            SecondCarMoves = secondCarMoves;
        }
    }
}