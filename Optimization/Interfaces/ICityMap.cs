using System.Collections.Generic;

namespace Optimization.Interfaces
{
    /// <summary>
    /// Дорожная карта города.
    /// </summary>
    public interface ICityMap
    {
        /// <summary>
        /// Места в городе.
        /// </summary>
        ISet<ICityPlace> Places { get; }

        /// <summary>
        /// Дороги в городе.
        /// </summary>
        ISet<ICityRoad> Roads { get; }
    }
}