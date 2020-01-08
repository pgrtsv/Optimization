using FluentValidation;
using Optimization.Core;

namespace Optimization.Validation
{
    public class VehicleModelValidator : AbstractValidator<IVehicleModel>
    {
        public static VehicleModelValidator Instance { get; } = new VehicleModelValidator();

        public VehicleModelValidator()
        {
            RuleFor(x => x.MaxVelocity)
                .GreaterThan(0);
            RuleFor(x => x.Dimensions)
                .Must(x => x.Item1 > 0)
                .Must(x => x.Item2 > 0)
                .Must(x => x.Item3 > 0);
            RuleFor(x => x.Capacity)
                .GreaterThan(0);
            RuleFor(x => x.RentalPrice)
                .GreaterThan(0);
        }
    }
}