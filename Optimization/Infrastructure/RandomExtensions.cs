using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimization.Infrastructure
{
    public static class RandomExtensions
    {
        public static List<int> GetDistinctRandomNumbers(this Random random, int count, int minValue, int maxValue)
        {
            if (maxValue <= minValue) throw new ArgumentException();
            if (count >= maxValue - minValue) throw new ArgumentException();
            var result = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                var num = random.Next(minValue, maxValue);
                while (result.Any(x => x.Equals(num)))
                    num = random.Next(minValue, maxValue);
                result.Add(num);
            }

            return result;
        }

        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            if (maxValue <= minValue) throw new ArgumentException();
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}