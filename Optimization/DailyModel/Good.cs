using FluentValidation;
using Optimization.Interfaces;
using Optimization.Validation;

namespace Optimization.DailyModel
{
    /// <inheritdoc />
    public class Good : IGood
    {
        public Good(double volume, decimal price, string name)
        {
            Volume = volume;
            Price = price;
            Name = name;
            GoodValidator.Instance.ValidateAndThrow(this);
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public double Volume { get; }

        /// <inheritdoc />
        public decimal Price { get; }

        protected bool Equals(Good other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Good) obj);
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }
    }
}