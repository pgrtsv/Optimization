using System;
using System.Linq;
using FluentValidation;
using Optimization.Core;
using Optimization.Validation;

namespace Optimization.DailyModel
{
    public class Vehicle : IVehicle
    {
        public Vehicle(int id, VehicleModel vehicleModel, string name, IWarehouse warehouse)
        {
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
        public Coordinate Position { get; }
        public IRoute Route { get; set; }

        private ICityRoad _currentRoad;

        private readonly Random _random = new Random();

        public void Move(TimeSpan timeSpan)
        {
            if (_currentRoad == null)
                _currentRoad = Route.Roads.First();
            //var velocity = 
            //    _currentRoad.
            //    _random.Next(0, 15) / 100.0 * MaxVelocity; // средняя скорость на дороге
            //var distance
        }

        private double GetVelocityOnRoad(RoadUsage roadUsage)
        {
            throw new NotImplementedException();
            //return roadUsage switch
            //{
            //    RoadUsage.High => 
                
            //};
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