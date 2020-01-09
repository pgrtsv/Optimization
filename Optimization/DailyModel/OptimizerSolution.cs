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

        /// <summary>
        /// Автомобиль для аренды.
        /// </summary>
        public IVehicle Vehicle { get; }

        /// <summary>
        /// Маршрут следования ТС.
        /// </summary>
        public IRoute Route { get; }

        /// <summary>
        /// Товары, загруженные в ТС.
        /// </summary>
        public IDictionary<IGood, int> Goods { get; }
    }
}