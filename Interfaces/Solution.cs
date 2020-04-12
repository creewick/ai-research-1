using System.Text.Json.Serialization;
using AI_Research_1.Helpers;

namespace AI_Research_1.Interfaces
{
    public class Solution
    {
        [JsonPropertyName("Car1")] public V[] FirstCarMoves { get; }
        [JsonPropertyName("Car2")] public V[] SecondCarMoves { get; }
        
        public Solution(V[] firstCarMoves, V[] secondCarMoves)
        {
            FirstCarMoves = firstCarMoves;
            SecondCarMoves = secondCarMoves;
        }
    }
}