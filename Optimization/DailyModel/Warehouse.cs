using Optimization.Interfaces;

namespace Optimization.DailyModel
{
    public class Warehouse: IWarehouse
    {
        public Warehouse((double, double) coordinates)
        {
            Coordinates = coordinates;
        }

        public (double, double) Coordinates { get; }
    }
}