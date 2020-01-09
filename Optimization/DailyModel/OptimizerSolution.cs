using System.Collections.Generic;
using Optimization.Core;

namespace Optimization.DailyModel
{
    public class OptimizerSolution: IOptimizerSolution
    {
        public OptimizerSolution(IVehicle vehicle, IRoute route, IDictionary<IGood, int> goods)
        {
            Vehicle = vehicle;
            Route = route;
            Goods = goods;
            Vehicle.Route = route;
        }

        public IVehicle Vehicle { get; }
        public IRoute Route { get; }
        public IDictionary<IGood, int> Goods { get; }
    }
}