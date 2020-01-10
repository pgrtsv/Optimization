using System;
using System.Collections.Generic;

namespace Optimization.Core
{
    public interface IOptimizer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="availableVehicles">Доступные для аренды ТС.</param>
        /// <param name="dailyOrders">Заказы торговых точек.</param>
        /// <param name="date">Дата, на которую нужно выбрать маршруты и ТС и расперделить грузы.</param>
        /// <returns></returns>
       List<IOptimizerSolution> Solve(
            IEnumerable<IVehicle> availableVehicles,
            IEnumerable<IOrder> dailyOrders,
            DateTime date);

        /// <summary>
        /// Карта города.
        /// </summary>
        ICityMap CityMap { get; }
    }
}