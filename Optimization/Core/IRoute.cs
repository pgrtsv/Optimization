using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimization.Core
{
    /// <summary>
    /// Маршрут.
    /// </summary>
    public interface IRoute
    {
        ICityPlace Start { get; }
        ICityPlace End { get; }
        IReadOnlyCollection<ICityRoad> Roads { get; }
    }

    public static class RouteExtensions
    {
        public static double Distance(this IRoute route)
        {
            return route.Roads.Sum(x => x.GetDistance());
        }
    }
}