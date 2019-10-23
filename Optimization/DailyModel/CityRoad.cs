using FluentValidation;
using Optimization.Interfaces;
using Optimization.Validation;

namespace Optimization.DailyModel
{
    public class CityRoad: ICityRoad
    {
        public CityRoad(ICityPlace firstPlace, ICityPlace secondPlace, double distance, RoadUsage usage)
        {
            FirstPlace = firstPlace;
            SecondPlace = secondPlace;
            Distance = distance;
            Usage = usage;
            CityRoadValidator.Instance.ValidateAndThrow(this);
        }

        public ICityPlace FirstPlace { get; }
        public ICityPlace SecondPlace { get; }
        public double Distance { get; }
        public RoadUsage Usage { get; }
    }
}