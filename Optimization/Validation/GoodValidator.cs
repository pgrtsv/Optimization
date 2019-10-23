using FluentValidation;
using Optimization.Interfaces;

namespace Optimization.Validation
{
    public class GoodValidator : AbstractValidator<IGood>
    {
        public GoodValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThan(0);
            RuleFor(x => x.Volume)
                .GreaterThan(0);
        }

        public static GoodValidator Instance { get; } = new GoodValidator();
    }
}