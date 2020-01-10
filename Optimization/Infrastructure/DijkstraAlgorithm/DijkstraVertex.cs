using System;
using Optimization.Core;

namespace Optimization.Infrastructure.DijkstraAlgorithm
{
    /// <summary>
    /// Вершина графа с информацией для алгоритма Дейкстры
    /// </summary>
    public class DijkstraVertex
    {
        public DijkstraVertex(ICityPlace cityPlace)
        {
            CityPlace = cityPlace;

            IsVisited = false;
            EdgesWeightSum = double.MaxValue;
            PreviousCityPlace = null;
        }

        /// <summary>
        /// Текущая вершина графа.
        /// </summary>
        public ICityPlace CityPlace { get; }

        /// <summary>
        /// <code>true</code> - если это проверенная на оптимальные маршруты вершина.
        /// </summary>
        public bool IsVisited { get; set; }

        /// <summary>
        /// Сумма весов ребер, которые необходимо пройти следую из исходной точки к текущей.
        /// </summary>
        public double EdgesWeightSum { get; set; }

        /// <summary>
        /// Предыдущая вершина графа.
        /// </summary>
        public ICityPlace PreviousCityPlace { get; set; }
    }
}