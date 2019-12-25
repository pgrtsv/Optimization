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

        public double X { get; }

        public double Y { get; }

        public double DistanceTo(Coordinate coordinate) =>
            Math.Sqrt(Math.Pow(X - coordinate.X, 2) + Math.Pow(Y - coordinate.Y, 2));
    }
}