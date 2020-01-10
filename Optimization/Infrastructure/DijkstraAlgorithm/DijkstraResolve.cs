using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Core;
using Optimization.DailyModel;

namespace Optimization.Infrastructure.DijkstraAlgorithm
{
    public class DijkstraResolve
    {
        private ISet<DijkstraVertex> _dijkstraVertices;

        public DijkstraResolve(ISet<DijkstraVertex> dijkstraVertices, 
            ICityMap cityMap, ICityPlace startPlace)
        {
            _dijkstraVertices = dijkstraVertices;
            CityMap = cityMap;
            StartPlace = startPlace;
        }

        public ICityMap CityMap { get; }

        public ICityPlace StartPlace { get; }

        /// <summary>
        /// Находит ближайший путь до указанного местоположения.
        /// </summary>
        /// <param name="cityPlace">Местоположение до которого необходимо получить ближайший путь.</param>
        /// <returns>Путь <see cref="IRoute"/>.</returns>
        public IRoute FindShortRouteTo(ICityPlace cityPlace)
        {
            if(!CityMap.Places.Contains(cityPlace))
                throw new ArgumentException("Местоположение не относится к текущему городу.");

            var roads = new List<ICityRoad>();
            
            var vertex = _dijkstraVertices.GetDijkstraVertex(cityPlace);
            while (vertex.CityPlace != StartPlace)
            {
                var road = CityMap.GetRoadBetween(vertex.CityPlace, vertex.PreviousCityPlace);
                roads.Add(road);
                vertex = _dijkstraVertices.GetDijkstraVertex(vertex.PreviousCityPlace);
            }

            roads.Reverse();
            return new Route(StartPlace, cityPlace, roads);
        }
    }
}