using System;
using System.Linq;
using DynamicData;
using FluentValidation;
using Optimization.Core;
using Optimization.Validation;

namespace Optimization.DailyModel
{
    public class Vehicle : IVehicle
    {
        private readonly IWarehouse _warehouse;

        public Vehicle(int id, VehicleModel vehicleModel, string name, IWarehouse warehouse)
        {
            _warehouse = warehouse;
            VehicleModelValidator.Instance.ValidateAndThrow(vehicleModel);

            Id = id;
            VehicleModel = vehicleModel;
            Name = name;
            Capacity = VehicleModel.Capacity;
            MaxVelocity = VehicleModel.MaxVelocity;
            Dimensions = VehicleModel.Dimensions;
            RentalPrice = VehicleModel.MaxVelocity;
            Type = vehicleModel.Type;
            Position = warehouse.Coordinates;
        }

        public int Id { get; }
        public VehicleModel VehicleModel { get; }
        public Coordinate Position { get; private set; }
        public IRoute Route { get; set; }

        private ICityRoad _currentRoad;
        private bool _isDirect;


        private readonly Random _random = new Random();

        public void Move(TimeSpan timeSpan)
        {
            if (Position.Equals(Route.End.Coordinates))
                return;
            if (_currentRoad == null)
            {
                _currentRoad = Route.Roads.First();
                _isDirect = _currentRoad.FirstPlace.Equals(_warehouse);
            }

            var velocity = // средняя скорость на дороге
                _currentRoad.Usage switch
                {
                    RoadUsage.High => _random.Next(0, 15) / 100.0 * MaxVelocity,
                    RoadUsage.Medium => _random.Next(20, 60) / 100.0 * MaxVelocity,
                    RoadUsage.Low => _random.Next(80, 100) / 100.0 * MaxVelocity
                };
            var distance = velocity / 60; // расстояние, которое проедет авто за данное время на текущей дороге с полученной средней скоростью.
            var currentRoadEnd = _isDirect ? _currentRoad.SecondPlace.Coordinates : _currentRoad.FirstPlace.Coordinates;
            var distanceToRoadEnd = Position.DistanceTo(currentRoadEnd);
            if (distanceToRoadEnd <= distance)
            {
                Position = currentRoadEnd;
                if (_currentRoad.Equals(Route.Roads.Last()))
                    return;
                var currentRoadIndex = Route.Roads.IndexOf(_currentRoad);
                _currentRoad = Route.Roads.ElementAt(currentRoadIndex + 1);
                _isDirect = _currentRoad.FirstPlace.Coordinates.Equals(currentRoadEnd);
                return;
            }

            Position = _isDirect
                ? Position.MoveBetween(_currentRoad.FirstPlace.Coordinates, _currentRoad.SecondPlace.Coordinates,
                    distance)
                : Position.MoveBetween(_currentRoad.SecondPlace.Coordinates, _currentRoad.FirstPlace.Coordinates,
                    distance);
        }

        public string Name { get; }
        public double Capacity { get; }
        public double MaxVelocity { get; }
        public (double, double, double) Dimensions { get; }
        public VehicleType Type { get; }
        public double RentalPrice { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}