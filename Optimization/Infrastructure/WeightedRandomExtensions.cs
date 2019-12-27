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
        public static bool NextBool(this Random random, int trueWeight, int falseWeight)
        {
            var randomValue = random.Next(trueWeight + falseWeight);
            return randomValue < trueWeight;
        }

        /// <summary>
        /// Генерирует случайный диапазон.
        /// </summary>
        /// <param name="random"><see cref="Random"/>.</param>
        /// <param name="minValue">Минимальное значение диапазона.</param>
        /// <param name="maxValue">Максимальное значение диапазона.</param>
        /// <param name="weights">Массив весов.</param>
        /// <returns>Случайный диапазон.</returns>
        public static (int, int) NextIntRange(this Random random, int minValue, int maxValue,
            int[] weights)
        {
            var weightRangeIndex = random.GetWeightIndexFrom(weights);

            /* Находим границы диапазона возможных чисел, относящихся к текущему весу. */
            var valueOnWeightUnit = (maxValue - minValue) / weights.Sum();
            var minOfRange = minValue;
            if (weightRangeIndex != 0)
                for (int i = 0; i < weightRangeIndex; i++)
                    minOfRange += weights[i] * valueOnWeightUnit;
            var maxOfRange =
                minOfRange + weights[weightRangeIndex] * valueOnWeightUnit;

            return (minOfRange, maxOfRange);
        }

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