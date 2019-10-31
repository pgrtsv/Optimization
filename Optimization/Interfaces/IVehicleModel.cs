using System;

namespace Optimization.Interfaces
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
        /// Вместительность (объём груза, который можно загрузить в ТС), м3.
        /// </summary>
        double Capacity { get; }

        /// <summary>
        /// Время разгона авто от 0 до 100 км/ч.
        /// </summary>
        double AccelerationTime { get; }

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