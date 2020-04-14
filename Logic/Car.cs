using AI_Research_1.Helpers;
using AI_Research_1.Serialization;

namespace AI_Research_1.Logic
{
    public class Car : Disk, IValuesList
    {
        public V V { get; set; }
        public bool IsAlive { get; set; }

        public Car(V pos, V v, int radius, bool isAlive=true) : base(pos, radius)
        {
            IsAlive = isAlive;
            V = v;
        }
        
        public void Tick()
        {
            Pos += V;
        }

        public Car Copy() => new Car(Pos, V, Radius, IsAlive);

        public new object[] GetValuesList() => new object[] {Pos.X, Pos.Y, V.X, V.Y, IsAlive};
    }
}