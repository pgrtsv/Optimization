using FluentValidation;
using Optimization.Core;
using Optimization.Validation;

namespace Optimization.DailyModel
{
    public class CityRoad: ICityRoad
    {
        public CityRoad(ICityPlace firstPlace, ICityPlace secondPlace, RoadRank rank)
        {
            FirstPlace = firstPlace;
            SecondPlace = secondPlace;
            Rank = rank;
            CityRoadValidator.Instance.ValidateAndThrow(this);
        }

        public ICityPlace FirstPlace { get; }
        public ICityPlace SecondPlace { get; }
        public double GetDistance() => FirstPlace.Coordinates.DistanceTo(SecondPlace.Coordinates);
        public RoadRank Rank { get; }
    }
}