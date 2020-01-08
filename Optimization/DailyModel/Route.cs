using System.Collections.Generic;
using Optimization.Core;

namespace Optimization.DailyModel
{
    public class Route: IRoute
    {
        public Route(ICityPlace start, ICityPlace end, IReadOnlyCollection<ICityRoad> roads)
        {
            Start = start;
            End = end;
            Roads = roads;
        }

        public ICityPlace Start { get; }
        public ICityPlace End { get; }
        public IReadOnlyCollection<ICityRoad> Roads { get; }
    }
}