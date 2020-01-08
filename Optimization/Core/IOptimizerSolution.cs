using System.Collections.Generic;

namespace Optimization.Core
{
    public interface IOptimizerSolution
    {
        /// <summary>
        /// ТС.
        /// </summary>
        IVehicle Vehicle { get; }
        /// <summary>
        /// Маршрут.
        /// </summary>
        IRoute Route { get; }
        /// <summary>
        /// Товары в ТС.
        /// </summary>
        IDictionary<IGood, int> Goods { get; }
    }
}