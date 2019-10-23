using System.Collections.Generic;

namespace Optimization.Interfaces
{
    /// <summary>
    /// Заказ складу от точки сбыта.
    /// </summary>
    public interface IOrder
    {
        /// <summary>
        /// Точка сбыта.
        /// </summary>
        ISalePoint SalePoint { get; }

        /// <summary>
        /// Товары в заказе и их количество.
        /// </summary>
        IDictionary<IGood, int> NeededGoods { get; }
    }
}