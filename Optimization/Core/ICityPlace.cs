namespace Optimization.Core
{
    /// <summary>
    /// Место в городе.
    /// </summary>
    public interface ICityPlace
    {
        /// <summary>
        /// Координаты (X, Y) места.
        /// </summary>
        Coordinate Coordinates { get; }
    }
}