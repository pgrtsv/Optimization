using System;

namespace Optimization.Core
{
    /// <summary>
    /// Ранг (популярность) дороги.
    /// </summary>
    public enum RoadRank
    {
        /// <summary>
        /// Важная дорога.
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
        /// Ранг дороги.
        /// </summary>
        RoadRank Rank { get; }

        /// <summary>
        /// Текущая ситуация на дороге.
        /// </summary>
        RoadUsage Usage { get; }

        /// <summary>
        /// Генерирует ситуацию на дороге на момент времени <see cref="dateTime"/>.
        /// </summary>
        void GenerateRoadUsage(DateTime dateTime);
    }
}