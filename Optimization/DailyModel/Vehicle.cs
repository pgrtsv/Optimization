using System;
using System.Collections.Generic;
using System.Linq;
using DynamicData;
using FluentValidation;
using Optimization.Core;
using Optimization.Validation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Optimization.DailyModel
{
    public class Vehicle : ReactiveObject, IVehicle
    {
        private readonly IWarehouse _warehouse;

        public Vehicle(int id, VehicleModel vehicleModel, IWarehouse warehouse)
        {
            _warehouse = warehouse;
            VehicleModelValidator.Instance.ValidateAndThrow(vehicleModel);

            Id = id;
            VehicleModel = vehicleModel;
            Position = warehouse.Coordinates;
            Cargo = new Dictionary<IGood, int>();
            FreeCapacity = vehicleModel.Capacity;
            new VehicleValidator().ValidateAndThrow(this);
        }

        public int Id { get; }
        public VehicleModel VehicleModel { get; }
        public Coordinate Position { get; private set; }
        public IRoute Route { get; set; }
        [Reactive] public IDictionary<IGood, int> Cargo { get; private set; }

        public void SetCargo(IDictionary<IGood, int> cargo)
        {
            Cargo = cargo;
            new VehicleValidator().ValidateAndThrow(this);
        }

        private ICityRoad _currentRoad;
        private bool _isDirect;


        private readonly Random _random = new Random();

        public void Move(TimeSpan timeSpan)
        {
            if (Route == null)
                return;
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
            var distance = velocity * 5 / 60; // расстояние, которое проедет авто за данное время на текущей дороге с полученной средней скоростью.
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

        public string Name => VehicleModel.Name;
        public double Capacity => VehicleModel.Capacity;
        public double MaxVelocity => VehicleModel.MaxVelocity;
        public (double, double, double) Dimensions => VehicleModel.Dimensions;
        public VehicleType Type => VehicleModel.Type;
        public double RentalPrice => VehicleModel.RentalPrice;
        public double FreeCapacity { get; set; }
        public List<IOrder> Orders { get; } = new List<IOrder>();

        public override string ToString()
        {
            return Name;
        }
    }
}