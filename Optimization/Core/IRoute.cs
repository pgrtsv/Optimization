using System;
using System.Collections.Generic;

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
            throw new NotImplementedException();
        }
    }
}