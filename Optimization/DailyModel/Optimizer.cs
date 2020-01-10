using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Core;
using Optimization.Infrastructure.DijkstraAlgorithm;

namespace Optimization.DailyModel
{
    public class Optimizer : IOptimizer
    {
        private readonly DijkstraResolve _dijkstraResolve;

        public Optimizer(ICityMap cityMap)
        {
            CityMap = cityMap;
            _dijkstraResolve = new DijkstraResolver()
                .Resolve(cityMap,
                    cityMap.Places.First(x => x is IWarehouse));
        }

        public ICityMap CityMap { get; }

        public List<IOptimizerSolution> Solve(
            IEnumerable<IVehicle> availableVehicles,
            IEnumerable<IOrder> dailyOrders,
            DateTime date)
        {
            return GetVehiclesWithOrder(availableVehicles, dailyOrders)
                .Select(x =>
                    new OptimizerSolution(x, _dijkstraResolve.FindShortRouteTo(x.Orders.First().SalePoint),
                        x.Cargo))
                .Cast<IOptimizerSolution>()
                .ToList();
        }

        public IEnumerable<IVehicle> GetVehiclesWithOrder(IEnumerable<IVehicle> availableVehicles,
            IEnumerable<IOrder> dailyOrders)
        {
            var chooseVechicle = new List<IVehicle>();
            foreach (var order in dailyOrders)
            {
                double V = 0;
                foreach (var item in order.NeededGoods) V += item.Key.Volume * item.Value;
                foreach (var vehicle in availableVehicles)
                    if (vehicle.FreeCapacity >= V)
                    {
                        vehicle.FreeCapacity -= V;
                        vehicle.Orders.Add(order);
                        foreach (var good in order.NeededGoods)
                            if (vehicle.Cargo.ContainsKey(good.Key))
                                vehicle.Cargo[good.Key] += good.Value;
                            else
                                vehicle.Cargo.Add(good.Key, good.Value);
                        V = -1;
                        break;
                    }

                if (V != -1)
                {
                    continue;
                    throw new Exception("Для заказа " + order + "не может быть найдена машина");
                }
            }

            foreach (var vehicle in availableVehicles)
                if (vehicle.Orders.Count != 0)
                    chooseVechicle.Add(vehicle);
            return chooseVechicle;
        }
    }
}