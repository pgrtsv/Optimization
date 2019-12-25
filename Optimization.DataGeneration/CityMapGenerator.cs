using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Core;
using Optimization.DailyModel;
using Optimization.Infrastructure;

namespace Optimization.DataGeneration
{
    public class CityMapGenerator
    {
        public CityMap Generate(IGoods goods)
        {
            var warehouse = GenerateWarehouse();
            var salePoints = GenerateSalePoints(10, 1000, warehouse, goods);
            var randomCityPlaces = GenerateCityPlaces(5, 500, warehouse);
            var cityPlaces = salePoints.Cast<ICityPlace>().Union(randomCityPlaces).Union(new[] {warehouse}).ToHashSet();
            var roads = GenerateCityRoads(cityPlaces);
            return new CityMap(
                cityPlaces,
                roads.Cast<ICityRoad>().ToHashSet()
            );
        }

        private Warehouse GenerateWarehouse()
        {
            return new Warehouse(new Coordinate(0, 0));
        }

        private HashSet<SalePoint> GenerateSalePoints(
            int count,
            double maxDistanceFromWarehouse,
            IWarehouse warehouse,
            IGoods goods)
        {
            var random = new Random();
            return Enumerable.Range(0, count).Select(i =>
            {
                var r = maxDistanceFromWarehouse * Math.Sqrt(random.NextDouble());
                var theta = random.NextDouble() * 2 * Math.PI;
                return new SalePoint(
                    new Coordinate(warehouse.Coordinates.X + r * Math.Cos(theta),
                        warehouse.Coordinates.Y + r * Math.Sin(theta)),
                    VehicleType.Passenger | VehicleType.BigTruck,
                    goods
                );
            }).ToHashSet();
        }

        private HashSet<CityPlace> GenerateCityPlaces(
            int count,
            double maxDistanceFromWarehouse,
            IWarehouse warehouse)
        {
            var random = new Random();
            return Enumerable.Range(0, count).Select(i =>
            {
                var r = maxDistanceFromWarehouse * Math.Sqrt(random.NextDouble());
                var theta = random.NextDouble() * 2 * Math.PI;
                return new CityPlace(
                    new Coordinate(warehouse.Coordinates.X + r * Math.Cos(theta),
                        warehouse.Coordinates.Y + r * Math.Sin(theta))
                );
            }).ToHashSet();
        }

        private HashSet<CityRoad> GenerateCityRoads(ICollection<ICityPlace> cityPlaces)
        {
            var random = new Random();
            var randomizedCityPlaces = random.RandomSort(cityPlaces).ToList();
            var roads = new HashSet<CityRoad>();
            for (var i = 1; i < randomizedCityPlaces.Count; i++)
                roads.Add(new CityRoad(randomizedCityPlaces[i - 1], randomizedCityPlaces[i],
                    random.NextDouble() < 0.5 ? RoadUsage.Medium : RoadUsage.High));
            for (var i = 0; i < 3; i++)
            for (int j = 0, jEnd = random.Next(0, randomizedCityPlaces.Count); j < jEnd; j++)
            {
                var firstPlace = randomizedCityPlaces.ElementAt(random.Next(0, randomizedCityPlaces.Count));
                var secondPlace = randomizedCityPlaces.ElementAt(random.Next(0, randomizedCityPlaces.Count));
                while (secondPlace == firstPlace)
                    secondPlace = randomizedCityPlaces.ElementAt(random.Next(0, randomizedCityPlaces.Count));
                var road = new CityRoad(
                    firstPlace,
                    secondPlace,
                    random.NextDouble() < 0.5 ? RoadUsage.Low : RoadUsage.Medium);
                if (roads.Any(x => x.FirstPlace.Equals(road.FirstPlace) && x.SecondPlace.Equals(road.SecondPlace)))
                    continue;
                roads.Add(road);
            }

            return roads;
        }
    }
}