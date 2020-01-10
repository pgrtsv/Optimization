using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Core;
using Optimization.Infrastructure.DijkstraAlgorithm;

namespace Optimization.DailyModel
{
    public class Optimizer: IOptimizer
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
            IEnumerable<IVehicle> chooseVehicles = GetVehiclesWithOrder(availableVehicles, dailyOrders);
            var ret = new List<IOptimizerSolution>();
            // TODO: бахнуть "оптимизатор".
            return ret;
        }

        public IEnumerable<IVehicle> GetVehiclesWithOrder(IEnumerable<IVehicle> availableVehicles, IEnumerable<IOrder> dailyOrders)
        {
            List<IVehicle> chooseVechicle = new List<IVehicle>();
            foreach(var order in dailyOrders)
            {
                double V = 0;
                foreach (var item in order.NeededGoods)
                {
                    V += item.Key.Volume * item.Value;
                }
                foreach(var vec in availableVehicles)
                {
                    if (vec.FreeCapacity >= V)
                    {
                        vec.FreeCapacity -= V;
                        vec.Orders.Add(order);
                        V = -1;
                        break;
                    }
                }
                if (V != -1)
                {
                    throw new Exception("Для заказа "+ order.ToString() + "не может быть найдена машина");
                }
            }
            foreach(var vec in availableVehicles)
            {
                if (vec.Orders.Count != 0)
                {
                    chooseVechicle.Add(vec);
                }
            }
            return chooseVechicle;
        }
    }
}