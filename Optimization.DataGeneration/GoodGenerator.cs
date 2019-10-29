using System;
using System.Collections.Generic;
using Optimization.DailyModel;
using Optimization.Infrastructure;

namespace Optimization.DataGeneration
{
    /// <summary>
    /// Класс-генератор товаров.
    /// </summary>
    public class GoodGenerator
    {
        private static readonly Random _random = new Random();

        #region Значения границ свойст товара при генерации по умолчанию

        public const int MinPrice = 20;

        public const int MaxPrice = 40000;

        public const double MinVolume = 0.00001;

        public const double MaxVolume = 10;

        #endregion

        /// <summary>
        /// Генерирует перечисление уникальных товаров.
        /// </summary>
        public static IEnumerable<Good> GenerateUniqueGoods(int count)
        {
            /* Для простоты названия товаров – числовой идентификатор. */
            /* Генерируем уникальные числовые индетификаторы. */
            var digitNumbers = count.ToString().Length;
            var minValue = (int) Math.Pow(10, digitNumbers);
            var maxValue = (int) Math.Pow(10, digitNumbers + 1) - 1;
            var uniqueNumbers = _random.GetDistinctRandomNumbers(count, minValue, maxValue);
            
            for (int i = 0; i < count; i++)
            {
                var name = uniqueNumbers[i].ToString();
                var price = _random.Next(MinPrice, MaxPrice);
                var volume = _random.NextDouble(MinVolume, MaxVolume);
                yield return new Good(volume, price, name);
            }
        }
    }
}