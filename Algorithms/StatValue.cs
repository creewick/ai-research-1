using System;
using System.Collections.Generic;
using System.Globalization;

namespace AiAlgorithms.Algorithms
{
    public class StatValue
    {
        public StatValue()
        {
        }

        public StatValue(IEnumerable<double> values)
        {
            foreach (var value in values) Add(value);
        }
        
        public StatValue(IEnumerable<int> values)
        {
            foreach (var value in values) Add(value);
        }

        public StatValue(long count, double sum, double sum2, double min, double max)
        {
            Count = count;
            Sum = sum;
            Sum2 = sum2;
            Min = min;
            Max = max;
        }

        public long Count { get; private set; }
        public double Sum { get; private set; }
        public double Sum2 { get; private set; }
        public double Min { get; private set; } = double.PositiveInfinity;
        public double Max { get; private set; } = double.NegativeInfinity;

        public double StdDeviation => Math.Sqrt(Count * Sum2 - Sum * Sum) / Count;

        /// <summary>
        ///     2 sigma confidence interval for mean value of random value
        /// </summary>
        public double ConfIntervalSize => 2 * Math.Sqrt(Count * Sum2 - Sum * Sum) / Count / Math.Sqrt(Count);

        public double Mean => Sum / Count;


        public void Add(double value)
        {
            Count++;
            Sum += value;
            Sum2 += value * value;
            Min = Math.Min(Min, value);
            Max = Math.Max(Max, value);
        }

        public void AddAll(StatValue value)
        {
            Count += value.Count;
            Sum += value.Sum;
            Sum2 += value.Sum2;
            Min = Math.Min(Min, value.Min);
            Max = Math.Max(Max, value.Max);
        }

        public string FormatCompact(double x)
        {
            if (Math.Abs(x) > 100) return x.ToString("0");
            if (Math.Abs(x) > 10) return x.ToString("0.#");
            if (Math.Abs(x) > 1) return x.ToString("0.##");
            if (Math.Abs(x) > 0.1) return x.ToString("0.###");
            if (Math.Abs(x) > 0.01) return x.ToString("0.####");
            return x.ToString();
        }

        public override string ToString()
        {
            return $"{FormatCompact(Mean)} sigma={FormatCompact(StdDeviation)}";
        }

        public string ToDetailedString(bool humanReadable = true)
        {
            if (humanReadable)
                return $"{Mean.ToString(CultureInfo.InvariantCulture)} " +
                       $"disp={StdDeviation.ToString(CultureInfo.InvariantCulture)} " +
                       $"min..max={Min.ToString(CultureInfo.InvariantCulture)}..{Max.ToString(CultureInfo.InvariantCulture)} " +
                       $"confInt={ConfIntervalSize.ToString(CultureInfo.InvariantCulture)} " +
                       $"count={Count}";
            FormattableString line = $"{Mean}\t{StdDeviation}\t{ConfIntervalSize}\t{Count}";
            return line.ToString(CultureInfo.InvariantCulture);
        }


        public StatValue Clone()
        {
            return new StatValue(Count, Sum, Sum2, Min, Max);
        }
    }
}