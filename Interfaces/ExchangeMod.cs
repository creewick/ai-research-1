using AI_Research_1.Logic;

namespace AI_Research_1.Interfaces
{
    public class ExchangeMod
    {
        private static int _cooldown = 20;
        public int Cooldown { get; private set; }

        public void Tick()
        {
            if (Cooldown > 0) Cooldown -= 1;
        }

        public void Apply(Car first, Car second)
        {
            Cooldown = _cooldown;
            first.ExchangeWith(second);
        }

        public ExchangeMod Copy() => new ExchangeMod {Cooldown = this.Cooldown};
    }
}