using System;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.racing
{
    public class RaceSolution : ISolution, IHaveTime, IHaveIndex, IMutation<RaceSolution>
    {
        public readonly V[] Accelerations;

        public RaceSolution(V[] accelerations)
        {
            Accelerations = accelerations;
        }

        public RaceSolution Clone()
        {
            return new RaceSolution(Accelerations)
            {
                Score = Score,
                Time = Time,
                ImprovementIndex = ImprovementIndex,
                MutationIndex = MutationIndex,
                Hint = Hint
            };
        }

        public override string ToString()
        {
            return $"Score: {Score}, Accelerations: {Accelerations.StrJoin(" ")} Improvement {ImprovementIndex} of {MutationIndex} mutations in {Time}";
        }

        protected bool Equals(RaceSolution other)
        {
            return Accelerations.SequenceEqual(other.Accelerations);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RaceSolution) obj);
        }

        public override int GetHashCode()
        {
            return (Accelerations != null ? Accelerations.GetHashCode() : 0);
        }

        public static bool operator ==(RaceSolution left, RaceSolution right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RaceSolution left, RaceSolution right)
        {
            return !Equals(left, right);
        }

        public double Score { get; set; }
        public RaceSolution GetResult() => this;
        public TimeSpan Time { get; set; }
        public int MutationIndex { get; set; }
        public int ImprovementIndex { get; set; }
        public string Hint { get; set; } = "";
    }
}