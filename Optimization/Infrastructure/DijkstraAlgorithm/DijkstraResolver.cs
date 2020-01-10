using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Core;

namespace Optimization.Infrastructure.DijkstraAlgorithm
{
    /// <summary>
    /// Класс представляющие алгоритм Дейкстры для поиска кратчайшего пути в графе между двумя точками.
    /// </summary>
    public class DijkstraResolver
    {

        private ISet<DijkstraVertex> _vertexes;
        private ICityMap _cityMap;

        public DijkstraResolve Resolve(ICityMap cityMap, ICityPlace startPoint)
        {
            _cityMap = cityMap;
            _vertexes = InitDijkstraVertexes(cityMap.Places);

            _vertexes
                .First(x => x.CityPlace == startPoint)
                .EdgesWeightSum = 0;

            while (true)
            {
                var current = FindUnvisitedVertexWithMinSum();
                if (current == null)
                {
                    break;
                }

                SetOptimalWaysForNeighbor(current);
            }

            return new DijkstraResolve(_vertexes, cityMap, startPoint);
        }

        private ISet<DijkstraVertex> InitDijkstraVertexes(IEnumerable<ICityPlace> vertexes)
        {
            var ret = new HashSet<DijkstraVertex>();
            foreach (var vertex in vertexes)
                ret.Add(new DijkstraVertex(vertex));

            return ret;
        }

        /// <summary>
        /// Поиск непосещенной вершины с минимальным значением суммы
        /// </summary>
        /// <returns>Вершина графа с информацией для алгоритма Дейкстры <see cref="DijkstraVertex"/>.</returns>
        private DijkstraVertex FindUnvisitedVertexWithMinSum()
        {
            var minValue = double.MaxValue;
            DijkstraVertex minVertexInfo = null;
            foreach (var vertex in _vertexes)
            {
                if (!vertex.IsVisited && vertex.EdgesWeightSum < minValue)
                {
                    minVertexInfo = vertex;
                    minValue = vertex.EdgesWeightSum;
                }
            }

            return minVertexInfo;
        }

        /// <summary>
        /// Вычисление суммы весов ребер для следующей вершины
        /// </summary>
        /// <param name="vertex">Информация о текущей вершине</param>
        void SetOptimalWaysForNeighbor(DijkstraVertex vertex)
        {
            foreach (var cityPlace in _cityMap.GetNeighborCityPlaces(vertex.CityPlace))
            {
                var nextVertex = _vertexes.GetDijkstraVertex(cityPlace);
                var road = _cityMap.GetRoadBetween(vertex.CityPlace, nextVertex.CityPlace);
                
                var sum = vertex.EdgesWeightSum + road.Weight;
                if (!(sum < nextVertex.EdgesWeightSum)) continue;
                nextVertex.EdgesWeightSum = sum;
                nextVertex.PreviousCityPlace = vertex.CityPlace;
            }

            vertex.IsVisited = true;
        }
    }

}