using FluentValidation;
using Optimization.Core;
using Optimization.Validation;

namespace Optimization.DailyModel
{
    public class Vehicle : IVehicle
    {
        public Vehicle(int id, VehicleModel vehicleModel, string name)
        {
            VehicleModelValidator.Instance.ValidateAndThrow(vehicleModel);

            Id = id;
            VehicleModel = vehicleModel;
            Name = name;
            Capacity = VehicleModel.Capacity;
            AccelerationTime = VehicleModel.AccelerationTime;
            Dimensions = VehicleModel.Dimensions;
            RentalPrice = VehicleModel.AccelerationTime;
            Type = vehicleModel.Type;
        }

        public int Id { get; }
        public VehicleModel VehicleModel { get; }
        public string Name { get; }
        public double Capacity { get; }
        public double AccelerationTime { get; }
        public (double, double, double) Dimensions { get; }
        public VehicleType Type { get; }
        public double RentalPrice { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}