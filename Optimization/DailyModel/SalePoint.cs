using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Core;
using Optimization.Infrastructure;
using ReactiveUI.Fody.Helpers;

namespace Optimization.DailyModel
{
    public class SalePoint: ISalePoint
    {
        public const int MaxOrderSize = 10;

        private readonly IGoods _goods;
        private readonly Random _random = new Random();

        #region Константы для генерации заказа.

        /* Все массивы в этом регионе должны иметь равное количество элементов. */

        /// <summary>
        /// Массив весов для генерации заказа.
        /// </summary>
        private static readonly int[] GoodGenerationWeights = { 2, 5, 25, 40, 20, 6, 2 };

        /// <summary>
        /// Массив устанавливает границы объема заказа (в м^3).
        /// </summary>
        private static readonly (double, double)[] VolumeGenerationRanges =
            {(5,10), (10, 15), (15, 25), (25, 40), (40, 100), (100, 200), (200, 250)};

        #endregion

        public SalePoint(
            Coordinate coordinates, 
            VehicleType acceptableVehicleTypes,
            IGoods goods)
        {
            _goods = goods;
            Coordinates = coordinates;
            AcceptableVehicleTypes = acceptableVehicleTypes;
        }

        public Coordinate Coordinates { get; }
        public VehicleType AcceptableVehicleTypes { get; }
        [Reactive] public IOrder CurrentOrder { get; private set; }

        /// <summary>
        /// Генерирует заказ.
        /// </summary>
        public void GenerateOrder()
        {
            var neededGoods = new Dictionary<IGood, int>();
            /* Чтобы не вызывать каждый раз LINQ для выборки уже взятых в заказ товаров. */
            var selectedGoods = new List<IGood>();

            /* Генерируем границы объема в пределах которых будет сгенерирован заказ. */
            var weightIndex = _random.GetWeightIndexFrom(GoodGenerationWeights);
            var (minTotalVolume, maxTotalVolume) = VolumeGenerationRanges[weightIndex];
            var remainingMaxVolume = maxTotalVolume;

            /* Пока не войдем в сгенерированные границы объема заказа, продолжаем генерировать заказ. */
            while (remainingMaxVolume > minTotalVolume && remainingMaxVolume <= maxTotalVolume)
            {
                /* Если уже сгенерировано некоторое число товарных позиций, и уже находимся (но ещё не превышаем)
                 * границы объема заказа, то с некоторой вероятностью можем закончить генерацию. */
                if (neededGoods.Count > 10 && remainingMaxVolume > minTotalVolume)
                    if (!_random.NextBool(220/neededGoods.Count, (int) 0.9*neededGoods.Count))
                    {
                        CurrentOrder = new Order(this, neededGoods);
                        return;
                    }
                /* Если уже все доступные товары задействованы в заказе - возвращаем заказ. */
                if(_goods.Count == selectedGoods.Count)
                {
                    CurrentOrder = new Order(this, neededGoods);
                    return;
                }

                /* Выбираем случайный товар. */
                var good = _random.GetRandomFrom((IList<IGood>) _goods, selectedGoods);
                while(good.Volume > remainingMaxVolume)
                    good = _random.GetRandomFrom((IList<IGood>)_goods, selectedGoods);

                /* Генерируем количество данного товара в заказе. */
                var countGenerationWeights = GetGoodCountGenerationWeights(good);
                var count = GenerateGoodCount(good, remainingMaxVolume, countGenerationWeights);

                /* Небольшой костыль (так как генерация количества товара в заказе не идеальна):
                 * если вдруг текущее количество товаров – 1/5 от всего заказа, с большей вероятностью
                 * сокращаем это количество. */
                if (good.Volume * count > maxTotalVolume / 5
                    && count > 1
                    && _random.NextBool(95, 5))
                {
                    /* Определяем порядок числа. */
                    var countNumberOrder = 0;
                    while (count / Math.Pow(10, countNumberOrder) > 10)
                        countNumberOrder++;

                    /* Сокращаем количество товаров в несколько раз (случайная величина). */
                    var maxDivider = countNumberOrder == 0 ? 3 : (int)Math.Pow(10, countNumberOrder);
                    /* Подстраховка, если вдруг в результате сокращения и приведения к int – получили ноль,
                     * то сокращаем в два раза исходное количество. */
                    var reducedCount = count / _random.Next(2, maxDivider);
                    count = reducedCount > 0
                        ? reducedCount : count / 2;
                }

                remainingMaxVolume -= good.Volume * count;
                neededGoods.Add(good, count);
                selectedGoods.Add(good);
            }

            CurrentOrder = new Order(this, neededGoods);
        }

        /// <summary>
        /// Генерирует количество товара в заказе.
        /// </summary>
        /// <param name="good">Товар.</param>
        /// <param name="remainingVolume">Допустимый объем в заказе.</param>
        /// <param name="weights">Массив весов на количество товара.</param>
        /// <returns>Количество товара в заказе.</returns>
        private int GenerateGoodCount(IGood good, double remainingVolume, int[] weights)
        {
            var totalMaxCount = (int)(remainingVolume / good.Volume);

            /* Генерируем небольше 400 товаров. */
            if (totalMaxCount > 400)
                totalMaxCount = _random.Next(200, 400);

            var (minCountBorder, maxCountBorder) = _random.NextIntRange(1, totalMaxCount, weights);
            var count = _random.Next(minCountBorder, maxCountBorder);

            return count;
        }

        public int[] GetGoodCountGenerationWeights(IGood good)
        {
            /* Если товар относится к наименьшой по объемам группе -
             * больший вес на генерацию большего количество товаров,
             * если товар относится к большой по объемам группе -
             * больший вес на генерацию меньшего количества товаров. */
            var firstWeight = (int)(100 * good.Volume);
            var thirdWeight = (int)(2 / good.Volume);
            var secondWeight = (firstWeight + thirdWeight) / 4;

            return new[] { firstWeight, secondWeight, thirdWeight };
        }
    }
}