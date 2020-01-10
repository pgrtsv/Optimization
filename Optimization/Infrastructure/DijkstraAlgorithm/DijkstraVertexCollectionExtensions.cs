using System.Collections.Generic;
using System.Linq;
using Optimization.Core;

namespace Optimization.Infrastructure.DijkstraAlgorithm
{
    public static class DijkstraVertexCollectionExtensions
    {
        public static DijkstraVertex GetDijkstraVertex(this IEnumerable<DijkstraVertex> vertexes, 
            ICityPlace cityPlace)
        {
            return vertexes.First(x => x.CityPlace == cityPlace);
        }
    }
}