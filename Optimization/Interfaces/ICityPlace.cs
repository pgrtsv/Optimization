namespace Optimization.Interfaces
{
    /// <summary>
    /// Место в городе.
    /// </summary>
    public interface ICityPlace
    {
        /// <summary>
        /// Координаты (X, Y) места.
        /// </summary>
        (double, double) Coordinates { get; }
    }
}