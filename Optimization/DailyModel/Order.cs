using System.Collections.Generic;
using FluentValidation;
using Optimization.Core;
using Optimization.Validation;

namespace Optimization.DailyModel
{
    public class Order: IOrder
    {
        public Order(ISalePoint salePoint, IDictionary<IGood, int> neededGoods)
        {
            SalePoint = salePoint;
            NeededGoods = neededGoods;
            OrderValidator.Instance.ValidateAndThrow(this);
        }

        public ISalePoint SalePoint { get; }
        public IDictionary<IGood, int> NeededGoods { get; }
    }
}