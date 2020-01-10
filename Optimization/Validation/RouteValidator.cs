using System.Linq;
using FluentValidation;
using Optimization.Core;

namespace Optimization.Validation
{
    public class RouteValidator: AbstractValidator<IRoute>
    {
        public RouteValidator()
        {
            RuleFor(x => x.End).NotEqual(x => x.Start)
                .WithMessage("Закольцованный маршрут не имеет смысла.");
            RuleFor(x => x.Roads)
                .Must((route, roads) =>
                {
                    var firstRoad = roads.First();
                    return firstRoad.FirstPlace == route.Start || firstRoad.SecondPlace == route.Start;
                })
                .WithMessage("Первая дорога в маршруте не связана с началом маршрута.");
            RuleFor(x => x.Roads)
                .Must((route, roads) =>
                {
                    var lastRoad = roads.Last();
                    return lastRoad.FirstPlace == route.End || lastRoad.SecondPlace == route.End;
                })
                .WithMessage("Последняя дорога в маршруте не связана с концом маршрута.");
            RuleFor(x => x.Roads)
                .Must((route, roads) =>
                {
                    var firstRoad = route.Roads.First();
                    var previousLocation = firstRoad.FirstPlace == route.Start
                        ? firstRoad.SecondPlace
                        : firstRoad.FirstPlace;
                    for (int i = 1; i < roads.Count; i++)
                    {
                        var road = roads.ElementAt(i);
                        if (road.FirstPlace != previousLocation && road.SecondPlace != previousLocation)
                            return false;
                        previousLocation = road.FirstPlace == previousLocation ? road.SecondPlace : road.FirstPlace;
                    }

                    return true;
                })
                .WithMessage("Дороги маршрута разорваны.");
        }
    }
}