using AI_Research_1.Helpers;

namespace AI_Research_1.Logic
{
    public class Car : Disk
    {
        public V V { get; private set; }
        public bool IsAlive { get; set; }

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

        public void ExchangeWith(Car other)
        {
            var temp = V;
            V = other.V;
            other.V = temp;
        } 

        public Car Copy() => new Car(Pos, V, Radius, IsAlive);
    }
}