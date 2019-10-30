using FluentValidation;
using Optimization.Interfaces;
using Optimization.Validation;

namespace Optimization.DailyModel
{
    public class Vehicle: IVehicle
    {
        public Vehicle(double capacity, double accelerationTime, (double, double, double) dimensions, VehicleType type, double rentalPrice)
        {
            Capacity = capacity;
            AccelerationTime = accelerationTime;
            Dimensions = dimensions;
            Type = type;
            RentalPrice = rentalPrice;
            VehicleValidator.Instance.ValidateAndThrow(this);
        }

        public double Capacity { get; }
        public double AccelerationTime { get; }
        public (double, double, double) Dimensions { get; }
        public VehicleType Type { get; }
        public double RentalPrice { get; }
    }
}