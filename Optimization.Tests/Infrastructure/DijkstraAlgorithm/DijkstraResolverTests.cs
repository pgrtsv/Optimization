using System.Collections.Generic;
using Optimization.Core;
using Optimization.DailyModel;
using Optimization.Infrastructure.DijkstraAlgorithm;
using Optimization.Tests.TestModel;
using Xunit;

namespace Optimization.Tests.Infrastructure.DijkstraAlgorithm
{
    public class DijkstraResolverTests
    {

        [Fact]
        public void FindShortWayTest()
        {
            var a = new CityPlace(new Coordinate(0, 0));
            var b = new CityPlace(new Coordinate(1, 4));
            var c = new CityPlace(new Coordinate(2, -1));
            var d = new CityPlace(new Coordinate(5, -1));
            var e = new CityPlace(new Coordinate(6, 4));

            var ac = new SelfWeightedCityRoad(a, c, 2);
            var ab = new SelfWeightedCityRoad(a, b, 5);
            var cb = new SelfWeightedCityRoad(c, b, 2);
            var cd = new SelfWeightedCityRoad(c, d, 6);
            var be = new SelfWeightedCityRoad(b, e, 7);
            var de = new SelfWeightedCityRoad(d, e, 1);

            var cityPlaces = new HashSet<ICityPlace> {a, b, c, d, e};
            var cityRoads = new HashSet<ICityRoad> {ac, ab, cb, cd, be, de};
            var cityMap = new CityMap(cityPlaces, cityRoads);

            var resolver = new DijkstraResolver();
            var resolve = resolver.Resolve(cityMap, a);

            var expected = new Route(a, e, new [] { ac, cd, de });
            var actual = resolve.FindShortRouteTo(e);

            Assert.Equal(expected.Start, actual.Start);
            Assert.Equal(expected.End, actual.End);
            Assert.Equal(expected.Roads, actual.Roads);
        }
        
    }
}