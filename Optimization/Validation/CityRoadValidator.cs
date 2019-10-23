using FluentValidation;
using Optimization.Interfaces;

namespace Optimization.Validation
{
    public class CityRoadValidator: AbstractValidator<ICityRoad>
    {
        public static CityRoadValidator Instance { get; } = new CityRoadValidator();
        public CityRoadValidator()
        {
            RuleFor(x => x.Distance)
                .GreaterThan(0);
            RuleFor(x => x.FirstPlace)
                .NotNull()
                .NotEqual(x => x.SecondPlace);
            RuleFor(x => x.SecondPlace)
                .NotNull()
                .NotEqual(x => x.FirstPlace);
        }
    }
}