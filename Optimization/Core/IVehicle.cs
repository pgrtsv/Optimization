using System;
using System.Collections.Generic;
using Optimization.DailyModel;

namespace Optimization.Core
{
    /// <summary>
    /// Транспортное средство.
    /// </summary>
    public interface IVehicle: IVehicleModel
    {
        /// <summary>
        /// Уникальный идентификатор транспортного средства.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Модель транспортного средства.
        /// </summary>
        VehicleModel VehicleModel { get; }

        /// <summary>
        /// Позиция ТС.
        /// </summary>
        Coordinate Position { get; }

        /// <summary>
        /// Маршрут.
        /// </summary>
        IRoute Route { get; set; }

        IDictionary<IGood, int> Cargo { get; set; }

        /// <summary>
        /// Двигается по маршруту <see cref="route"/> в течение времени <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="timeSpan">Время движения.</param>
        void Move(TimeSpan timeSpan);
    }
}