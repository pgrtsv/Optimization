using System;
using System.Collections.Generic;

namespace Optimization.Core
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

        /// <summary>
        /// Находит дороги, связанные с указанным местоположением.
        /// </summary>
        /// <param name="cityPlace">Местоположение в городе.</param>
        /// <returns>Перечисление дорог, связанные с указанным местоположением.</returns>
        /// <exception cref="ArgumentException">Если местоположение
        /// не принадлежит данному городу.</exception>
        IEnumerable<ICityRoad> GetRoadsFrom(ICityPlace cityPlace);

        /// <summary>
        /// Находит ближайшие местоположения, связанные с текущим местоположением.
        /// </summary>
        /// <param name="cityPlace">Местоположение.</param>
        /// <returns>Перечисление ближайших связанных местоположений.</returns>
        /// <exception cref="ArgumentException">Если местоположение
        /// не принадлежит данному городу.</exception>
        IEnumerable<ICityPlace> GetNeighborCityPlaces(ICityPlace cityPlace);

        //TODO: выиграли бы в производительности, если бы местоположения знали о дорогоах, которые из них идут.
        /// <summary>
        /// Возвращает дорогу между указанными местоположениями.
        /// </summary>
        /// <param name="firstCityPlace">Первое местоположение.</param>
        /// <param name="secondCityPlace">Второе местоположение.</param>
        /// <returns>Дорога между местоположениями, или null,
        /// если между местоположениями нет дороги.</returns>
        ICityRoad GetRoadBetween(ICityPlace firstCityPlace, ICityPlace secondCityPlace);
    }
}