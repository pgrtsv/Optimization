using System;

namespace Optimization.Core
{
    /// <summary>
    /// Тип транспортного средства.
    /// </summary>
    [Flags]
    public enum VehicleType
    {
        /// <summary>
        /// Легковой автомобиль.
        /// </summary>
        Passenger,
        /// <summary>
        /// Лёгкий грузовик.
        /// </summary>
        SmallTruck,
        /// <summary>
        /// Грузовик.
        /// </summary>
        BigTruck
    }

    /// <summary>
    /// Модель транспортного средства.
    /// </summary>
    public interface IVehicleModel
    {
        /// <summary>
        /// Название модели.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Вместительность (объём груза, который можно загрузить в ТС), м3.
        /// </summary>
        double Capacity { get; }

        /// <summary>
        /// Максимальная скорость ТС, км/ч.
        /// </summary>
        double MaxVelocity { get; }

        /// <summary>
        /// Габариты ТС (ДxШxВ).
        /// </summary>
        (double, double, double) Dimensions { get; }

        /// <summary>
        /// Тип ТС.
        /// </summary>
        VehicleType Type { get; }

        /// <summary>
        /// Цена аренды ТС за сутки.
        /// </summary>
        double RentalPrice { get; }
    }
}