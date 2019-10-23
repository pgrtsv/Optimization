using FluentValidation;
using Optimization.Interfaces;

namespace Optimization.Validation
{
    public class VehicleValidator : AbstractValidator<IVehicle>
    {
        public static VehicleValidator Instance { get; } = new VehicleValidator();

        public VehicleValidator()
        {
            RuleFor(x => x.AccelerationTime)
                .GreaterThan(0);
            RuleFor(x => x.Dimensions)
                .Must(x => x.Item1 > 0)
                .Must(x => x.Item2 > 0)
                .Must(x => x.Item3 > 0);
            RuleFor(x => x.Capacity)
                .GreaterThan(0);
        }
    }
}