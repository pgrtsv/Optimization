namespace Optimization.Core
{
    /// <summary>
    /// Точка сбыта.
    /// </summary>
    public interface ISalePoint: ICityPlace
    {
        /// <summary>
        /// Типы ТС, которые может принять точка сбыта.
        /// </summary>
        VehicleType AcceptableVehicleTypes { get; }

        /// <summary>
        /// Создаёт заказ для склада.
        /// </summary>
        /// <returns>Заказ.</returns>
        IOrder GenerateOrder();
    }
}