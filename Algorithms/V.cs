using System;
using System.Globalization;

namespace AiAlgorithms.Algorithms
{
    public class V : IEquatable<V>
    {
        public static readonly V Zero = new V(0, 0);

        public readonly int X;
        public readonly int Y;

        public static V Parse(string s)
        {
            var parts = s.Split(',');
            return new V(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        public V(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(V other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((V)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(V left, V right) => Equals(left, right);
        public static bool operator !=(V left, V right) => !Equals(left, right);

        public long Len2 => (long)X * X + (long)Y * Y;

        public override string ToString()
        {
            return $"[{X.ToString(CultureInfo.InvariantCulture)},{Y.ToString(CultureInfo.InvariantCulture)}]";
        }

        public static implicit operator V(string s)
        {
            var parts = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var x = int.Parse(parts[0], CultureInfo.InvariantCulture);
            var y = int.Parse(parts[1], CultureInfo.InvariantCulture);
            return new V(x, y);
        }

        public static V operator +(V a, V b) => new V(a.X + b.X, a.Y + b.Y);
        public static V operator -(V a, V b) => new V(a.X - b.X, a.Y - b.Y);
        public static V operator -(V a) => new V(-a.X, -a.Y);
        public static V operator *(V a, int k) => new V(k * a.X, k * a.Y);
        public static V operator *(int k, V a) => new V(k * a.X, k * a.Y);
        public static V operator /(V a, int k) => new V(a.X / k, a.Y / k);
        public static long operator *(V a, V b) => a.X * b.X + a.Y * b.Y;
        public static long operator ^(V a, V b) => a.X * b.Y - a.Y * b.X;

        public long Dist2To(V point) => (this - point).Len2;

        public double DistTo(V b) => Math.Sqrt(Dist2To(b));
    }
}