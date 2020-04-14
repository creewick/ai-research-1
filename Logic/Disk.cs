using System.Text.Json.Serialization;
using AI_Research_1.Helpers;
using AI_Research_1.Serialization;

namespace AI_Research_1.Logic
{
    public class Disk : IValuesList
    {
        [JsonPropertyName("R")] public int Radius { get; }
        public V Pos { get; set; }
        
        public Disk(V pos, int radius)
        {
            Radius = radius;
            Pos = pos;
        }

        public bool Intersect(Disk disk)
        {
            var minDist = disk.Radius + Radius;
            return minDist * minDist >= (Pos - disk.Pos).Len2();
        }

        public bool Contains(V point) => (point - Pos).Len2() <= Radius * Radius;

        public object[] GetValuesList() => new object[] {Pos.X, Pos.Y, Radius};
    }
}