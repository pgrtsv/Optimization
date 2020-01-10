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
            var ret = new List<IOptimizerSolution>();
            // TODO: бахнуть "оптимизатор".
            return ret;
        }
    }
}