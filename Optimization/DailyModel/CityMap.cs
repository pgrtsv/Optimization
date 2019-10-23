using System.Collections.Generic;
using FluentValidation;
using Optimization.Interfaces;
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
    }
}