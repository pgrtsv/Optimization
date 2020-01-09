using System.Linq;
using FluentValidation;
using Optimization.Core;

namespace Optimization.Validation
{
    public class VehicleValidator: AbstractValidator<IVehicle>
    {
        public VehicleValidator()
        {
            RuleFor(x => x.Cargo)
                .Must((vehicle, cargo) =>
                    {
                        return cargo.Select(x => x.Key.Volume * x.Value).Sum() <= vehicle.VehicleModel.Capacity;
                    })
                .WithMessage("Объём груза больше вместительности ТС.");
        }
    }
}