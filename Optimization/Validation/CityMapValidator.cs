using FluentValidation;
using Optimization.Interfaces;

namespace Optimization.Validation
{
    public class CityMapValidator : AbstractValidator<ICityMap>
    {
        public static CityMapValidator Instance { get; } = new CityMapValidator();

        public CityMapValidator()
        {
            RuleFor(x => x.Places)
                .NotEmpty();
            RuleFor(x => x.Roads)
                .NotEmpty();
        }
    }
}