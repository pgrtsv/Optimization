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

        public static IEnumerable<T> GetRandomFrom<T>(this Random random, IList<T> list, int count)
        {
            if (count <= 0) throw new ArgumentException();
            if (list?.Count <= 0) throw new ArgumentException();

            for (int i = 0; i < count; i++)
            {
                var randomIndex = random.Next(0, list.Count - 1);
                yield return list[randomIndex];
            }
        }
    }
}