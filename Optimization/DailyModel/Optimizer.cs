using System;
using System.Collections.Generic;
using Optimization.Core;

namespace Optimization.DailyModel
{
    public class Optimizer: IOptimizer
    {
        public List<IOptimizerSolution> Solve(
            IEnumerable<IVehicle> availableVehicles, 
            IEnumerable<IOrder> dailyOrders, 
            ICityMap cityMap, 
            DateTime date)
        {
            var ret = new List<IOptimizerSolution>();
            return ret;
        }
    }
}