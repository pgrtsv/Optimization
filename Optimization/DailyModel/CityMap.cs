using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Optimization.Core;
using Optimization.Validation;

namespace Optimization.DailyModel
{
    public class CityMap: ICityMap
    {
        public CityMap(ISet<ICityPlace> places, ISet<ICityRoad> roads)
        {
            Places = places;
            Roads = roads;
            CityMapValidator.Instance.ValidateAndThrow(this);
        }

        public ISet<ICityPlace> Places { get; }
        public ISet<ICityRoad> Roads { get; }

        public IEnumerable<ICityRoad> GetRoadsFrom(ICityPlace cityPlace)
        {
            if(!Places.Contains(cityPlace))
                throw new ArgumentException("Местоположение не принадлжеит городу.");

            return Roads
                .Where(x => x.FirstPlace == cityPlace || x.SecondPlace == cityPlace)
                .Distinct();
        }

        public IEnumerable<ICityPlace> GetNeighborCityPlaces(ICityPlace cityPlace)
        {
            var roads = GetRoadsFrom(cityPlace).ToArray();

            return roads.Select(x => x.SecondPlace)
                .Union(roads.Select(x => x.FirstPlace))
                .Distinct()
                .Except(new[] {cityPlace});
        }

        public ICityRoad GetRoadBetween(ICityPlace firstCityPlace, ICityPlace secondCityPlace)
        {
            var road = Roads.FirstOrDefault(x =>
                x.FirstPlace == firstCityPlace && x.SecondPlace == secondCityPlace ||
                x.FirstPlace == secondCityPlace && x.SecondPlace == firstCityPlace);
            
            return road;
        }
    }
}