﻿using Optimization.DailyModel;

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
    }
}