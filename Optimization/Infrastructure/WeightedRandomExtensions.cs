using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimization.Infrastructure
{
    /// <summary>
    /// Методы расширения для <see cref="Random"/> в которых используются веса.
    /// </summary>
    public static class WeightedRandomExtensions
    {
        /// <summary>
        /// Возвращает индекс случайно выбранного веса.
        /// Чем больше вес, тем больше вероятность возвращения его индекса.
        /// Актуально, когда каждый вес приурочен к некоторым границам генерации.
        /// Таким образом можно получать индекс границ генерации для некоторой случайной величины.
        /// </summary>
        /// <param name="random"><see cref="Random"/>.</param>
        /// <param name="weights">Массив весов.</param>
        /// <returns>Индекс массива.</returns>
        public static int GetWeightIndexFrom(this Random random, int[] weights)
        {
            if (weights == null || weights.Length == 0)
                throw new ArgumentException("Веса не заданы.");

            var totalWeight = weights.Sum();
            var randomFromWeights = random.Next(0, totalWeight);
            var rightWeightBorder = weights[0];
            var weightIndex = 0;
            for (int i = 0; i < weights.Length - 1; i++)
            {
                weightIndex = i;
                if (randomFromWeights < rightWeightBorder) break;
                rightWeightBorder += weights[i + 1];
            }

            return weightIndex;
        }
    }
}