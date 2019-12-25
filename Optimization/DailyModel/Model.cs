using System.Collections.Generic;
using Optimization.Core;

namespace Optimization.DailyModel
{
    public class Model
    {
        public Model(IReadOnlyCollection<IVehicleModel> vehicleModels, IReadOnlyCollection<IVehicle> vehicles, ICityMap cityMap, IGoods goods)
        {
            VehicleModels = vehicleModels;
            Vehicles = vehicles;
            CityMap = cityMap;
            Goods = goods;
        }

        public IReadOnlyCollection<IVehicleModel> VehicleModels { get; }
        public IReadOnlyCollection<IVehicle> Vehicles { get; }
        public ICityMap CityMap { get; }
        public IGoods Goods { get; }

    }
}