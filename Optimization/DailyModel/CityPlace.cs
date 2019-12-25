using Optimization.Core;

namespace Optimization.DailyModel
{
    /// <summary>
    /// Случайное место в городе (перекрёсток).
    /// </summary>
    public class CityPlace: ICityPlace
    {
        public CityPlace(Coordinate coordinates)
        {
            Coordinates = coordinates;
        }

        public Coordinate Coordinates { get; }
    }
}