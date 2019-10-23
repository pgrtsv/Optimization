namespace Optimization.Interfaces
{
    /// <summary>
    /// Окружение.
    /// </summary>
    public interface IEnvironment
    {
        /// <summary>
        /// Карта города.
        /// </summary>
        ICityMap CityMap { get; }

        /// <summary>
        /// Сервис аренды транспорта.
        /// </summary>
        ILeaseService LeaseService { get; }

        /// <summary>
        /// Функция вычисления штрафа.
        /// </summary>
        /// <param name="good">Недоставленный товар.</param>
        /// <param name="delayHours">Время, в течение которого товар не был доставлен, часы.</param>
        /// <returns>Штраф, рубли.</returns>
        decimal GetPenalty(IGood good, int delayHours);
    }
}