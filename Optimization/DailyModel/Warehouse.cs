using Optimization.Core;

namespace Optimization.DailyModel
{
    public class Warehouse: IWarehouse
    {
        public Warehouse(Coordinate coordinates)
        {
            Coordinates = coordinates;
        }

        public Coordinate Coordinates { get; }
    }
}