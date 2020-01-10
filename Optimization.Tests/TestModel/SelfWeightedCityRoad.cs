using System;
using Optimization.Core;

namespace Optimization.Tests.TestModel
{
    public class SelfWeightedCityRoad: ICityRoad
    {
        public SelfWeightedCityRoad(ICityPlace firstPlace, ICityPlace secondPlace, double weight)
        {
            FirstPlace = firstPlace;
            SecondPlace = secondPlace;
            Weight = weight;
        }

        public ICityPlace FirstPlace { get; }
        public ICityPlace SecondPlace { get; }
        public double GetDistance() => FirstPlace.Coordinates.DistanceTo(SecondPlace.Coordinates);

        public RoadRank Rank { get; }
        public RoadUsage Usage { get; }
        public double Weight { get; }
        public void GenerateRoadUsage(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}