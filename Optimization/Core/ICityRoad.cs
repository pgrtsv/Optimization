namespace Optimization.Core
{
    /// <summary>
    /// Ситуация на дороге.
    /// </summary>
    public enum RoadUsage
    {
        /// <summary>
        /// Пробки.
        /// </summary>
        High,

        /// <summary>
        /// Средний трафик.
        /// </summary>
        Medium,

        /// <summary>
        /// Пустая дорога.
        /// </summary>
        Low
    }

    /// <summary>
    /// Дорога в городе.
    /// </summary>
    public interface ICityRoad
    {
        /// <summary>
        /// Первая точка, которую соединяет дорога.
        /// </summary>
        ICityPlace FirstPlace { get; }

        /// <summary>
        /// Вторая точка, которую соединяет дорога.
        /// </summary>
        ICityPlace SecondPlace { get; }

        /// <summary>
        /// Длина дороги.
        /// </summary>
        double GetDistance();

        /// <summary>
        /// Ситуация на дороге.
        /// </summary>
        RoadUsage Usage { get; }
    }
}