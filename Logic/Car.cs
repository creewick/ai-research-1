using AI_Research_1.Helpers;
using Newtonsoft.Json;

namespace AI_Research_1.Logic
{
    public class Car : Disk
    {
        public V V { get; private set; }
        [JsonIgnore] public bool IsAlive { get; set; }

        public Car(V pos, V v, int radius, bool isAlive=true) : base(pos, radius)
        {
            IsAlive = isAlive;
            V = v;
        }

        public void Tick(V nextCommand)
        {
            V += nextCommand;
            Pos += V;
        }

        public Car Copy() => new Car(Pos, V, Radius, IsAlive);
    }
}