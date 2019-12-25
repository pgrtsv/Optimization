using FluentValidation;
using Optimization.Core;

namespace Optimization.Validation
{
    public class OrderValidator : AbstractValidator<IOrder>
    {
        public static OrderValidator Instance { get; } = new OrderValidator();

        public OrderValidator()
        {
            RuleFor(x => x.SalePoint)
                .NotNull();
            RuleFor(x => x.NeededGoods)
                .NotEmpty();
        }
    }
}