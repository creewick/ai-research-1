using System;

namespace AI_Research_1.Helpers
 {
     public struct V : IEquatable<V>
     {
         public static V Zero = new V(0, 0);
         
         public readonly int X { get; }
         public readonly int Y { get; }

         public V(int x, int y)
         {
             X = x;
             Y = y;
         }

         public bool Equals(V other) => X.Equals(other.X) && Y.Equals(other.Y);

         public static bool operator ==(V left, V right) => Equals(left, right);
         public static bool operator !=(V left, V right) => !Equals(left, right);

         public long Len2() => (long) X * X + (long) Y * Y;

         public static V operator +(V a, V b) => new V(a.X + b.X, a.Y + b.Y);
         public static V operator -(V a, V b) => new V(a.X - b.X, a.Y - b.Y);
         public static V operator -(V a) => new V(-a.X, -a.Y);
         public static V operator *(V a, int k) => new V(k * a.X, k * a.Y);
         public static V operator *(int k, V a) => new V(k * a.X, k * a.Y);
         public static V operator /(V a, int k) => new V(a.X / k, a.Y / k);
         public static long operator *(V a, V b) => a.X * b.X + a.Y * b.Y;
         public static long operator ^(V a, V b) => a.X * b.Y - a.Y * b.X;

         public long Dist2To(V point) => (this - point).Len2();

         public double DistTo(V b) => Math.Sqrt(Dist2To(b));
         
         public bool SegmentCrossPoint(V a, V b, int crossDistance) => Dist2ToSegment(a, b) <= crossDistance * crossDistance;

         public double Dist2ToSegment(V a, V b)
         {
             // optimized for speed! No intermediate V objects. No sqrt calls
             var bax = b.X - a.X;
             var pax = X - a.X;
             var pbx = X - b.X;
             var bay = b.Y - a.Y;
             var pay = Y - a.Y;
             var pby = Y - b.Y;
             var cos1 = bax * pax + bay * pay;
             var cos2 = -bax * pbx - bay * pby;
             if (cos1 <= 0) return Dist2To(a);
             if (cos2 <= 0) return Dist2To(b);
             double vp = bax * pay - bay * pax;
             return vp * vp / a.Dist2To(b);
         }
     }
 }