using System.Collections.Generic;
using FluentValidation;
using Optimization.Core;
using Optimization.Validation;

namespace Optimization.DailyModel
{
    public class Route: IRoute
    {
        public Route(ICityPlace start, ICityPlace end, IReadOnlyCollection<ICityRoad> roads)
        {
            Start = start;
            End = end;
            Roads = roads;
            new RouteValidator().ValidateAndThrow(this);
        }

        public ICityPlace Start { get; }
        public ICityPlace End { get; }
        public IReadOnlyCollection<ICityRoad> Roads { get; }
    }
}