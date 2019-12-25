using FluentValidation;
using Optimization.Core;
using Optimization.Validation;

namespace Optimization.DailyModel
{
    public class Vehicle: IVehicle
    {
        public Vehicle(int id, VehicleModel vehicleModel)
        {
            VehicleModelValidator.Instance.ValidateAndThrow(vehicleModel);

            Id = id;
            VehicleModel = vehicleModel;
            Capacity = VehicleModel.Capacity;
            AccelerationTime = VehicleModel.AccelerationTime;
            Dimensions = VehicleModel.Dimensions;
            Type = VehicleModel.Type;
            RentalPrice = VehicleModel.AccelerationTime;
        }

        public int Id { get; }
        public VehicleModel VehicleModel { get; }
        public double Capacity { get; }
        public double AccelerationTime { get; }
        public (double, double, double) Dimensions { get; }
        public VehicleType Type { get; }
        public double RentalPrice { get; }
    }
}