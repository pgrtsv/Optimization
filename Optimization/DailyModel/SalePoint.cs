using System;
using System.Linq;
using Optimization.Infrastructure;
using Optimization.Interfaces;

namespace Optimization.DailyModel
{
    public class SalePoint: ISalePoint
    {
        public const int MaxOrderSize = 10;

        private readonly IGoods _goods;
        private readonly Random _random = new Random();

        public SalePoint(
            (double, double) coordinates, 
            VehicleType acceptableVehicleTypes,
            IGoods goods)
        {
            _goods = goods;
            Coordinates = coordinates;
            AcceptableVehicleTypes = acceptableVehicleTypes;
        }

        public (double, double) Coordinates { get; }
        public VehicleType AcceptableVehicleTypes { get; }

        /// <summary>
        /// Генерирует заказ.
        /// </summary>
        /// TODO: придумать правило генерации количества товара. 
        public IOrder GenerateOrder()
        {
            var orderSize = _random.Next(1, MaxOrderSize);
            var goodsIndexes = _random.GetDistinctRandomNumbers(orderSize, 0, _goods.Count - 1);
            return new Order(this, Enumerable.Range(0, orderSize)
                .Select(i => _goods.ElementAt(goodsIndexes[i]))
                .ToDictionary(x => x, x => _random.Next(1, 100)));
        }
    }
}