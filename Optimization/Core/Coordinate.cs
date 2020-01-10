using System;

namespace Optimization.Core
{
    public struct Coordinate
    {
        public Coordinate(double x, double y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Coordinate other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinate other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public double X { get; }

        public double Y { get; }

        public double DistanceTo(Coordinate coordinate)
        {
            return Math.Sqrt(Math.Pow(X - coordinate.X, 2) + Math.Pow(Y - coordinate.Y, 2));
        }

        /// <summary>
        /// Передвигает текущую точку на отрезке между <see cref="first" /> и <see cref="second" /> в сторону <see cref="second" />
        /// на расстояние <see cref="distance" />.
        /// </summary>
        public Coordinate MoveBetween(Coordinate first, Coordinate second, double distance)
        {
            if (Math.Abs(distance) < double.Epsilon)
                return this;
            var k = (first.Y - second.Y) / (first.X - second.X);
            if (double.IsInfinity(k))
                return new Coordinate(X, Y < second.Y ? Y + distance : Y - distance);
            var l = first.Y - k * first.X;
            var a = k * k + 1;
            var b = 2 * k * l - 2 * k * Y - 2 * X;
            var c = l * l + X * X + Y * Y - distance * distance - 2 * l * Y;
            var d = b * b - 4 * a * c;
            var x = X < second.X ? (-b + Math.Sqrt(d)) / (2 * a) : (-b - Math.Sqrt(d)) / (2 * a);
            var y = k * x + l;
            return new Coordinate(x, y);
        }
    }
}