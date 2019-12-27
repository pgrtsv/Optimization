using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Core;
using Optimization.Infrastructure;

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
        private static readonly int[] GoodGenerationWeights = { 2, 5, 30, 40, 15, 6, 2 };

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

        /// <summary>
        /// Генерирует заказ.
        /// </summary>
        public IOrder GenerateOrder()
        {
            var neededGoods = new Dictionary<IGood, int>();
            /* Чтобы не вызывать каждый раз LINQ для выборки уже взятых в заказ товаров. */
            var selectedGoods = new List<IGood>();

            var weightIndex = _random.GetWeightIndexFrom(GoodGenerationWeights);
            var (minTotalVolume, maxTotalVolume) = VolumeGenerationRanges[weightIndex];
            var remainingMaxVolume = maxTotalVolume;

            while (remainingMaxVolume > minTotalVolume && remainingMaxVolume <= maxTotalVolume)
            {
                if (neededGoods.Count > 1 && remainingMaxVolume > minTotalVolume)
                    if (!_random.NextBool(220/neededGoods.Count, (int) 0.9*neededGoods.Count))
                        return new Order(this, neededGoods);
                var good = _random.PeekRandomFrom((IList<IGood>) _goods, selectedGoods);
                var weights = GetGoodCountWeights(good);
                var count = GenerateGoodCount(good, remainingMaxVolume, weights);
                
                /* Чтобы не было малого количества типов товаров в заказе. */
                if (neededGoods.Count < 8 && _random.NextBool(90, 10))
                    while (good.Volume * count > (maxTotalVolume - minTotalVolume) / 3)
                        count /= 2;
                
                remainingMaxVolume -= good.Volume * count;
                neededGoods.Add(good, count);
                selectedGoods.Add(good);
            }

            return new Order(this, neededGoods);
        }

        private int GenerateGoodCount(IGood good, double remainingVolume, int[] weights)
        {
            var totalMaxCount = (int)(remainingVolume / good.Volume);
            var (minCountBorder, maxCountBorder) = _random.NextIntRange(1, totalMaxCount, weights);
            return _random.Next(minCountBorder, maxCountBorder);
        }

        public int[] GetGoodCountWeights(IGood good)
        {
            /* Если товар относится к наименьшой по объемам группе -
             * больший вес на генерацию большего количество товаров,
             * если товар относится к большой по объемам группе -
             * больший вес на генерацию меньшего количества товаров.*/
            var firstWeight = (int)(100 * good.Volume);
            var thirdWeight = (int)(2 / good.Volume);
            var secondWeight = (firstWeight + thirdWeight) / 4;

            return new[] { firstWeight, secondWeight, thirdWeight };
        }
    }
}