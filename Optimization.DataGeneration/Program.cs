using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Optimization.Core;
using Optimization.DailyModel;

namespace Optimization.DataGeneration
{
    class Program
    {
        private static string DataPath = @"..\..\..\..\Data\";
        private static Random Random = new Random();

        static void Main(string[] args)
        {
            var vehicleModels = new VehicleModelGenerator().GenerateUniqueVehicleModels(50).ToList();
            var goods = new Goods(GoodGenerator.GenerateUniqueGoods(100).Cast<IGood>().ToList());
            var cityMap = new CityMapGenerator().Generate(goods);
            var warehouse = cityMap.Places.OfType<IWarehouse>().First();
            var vehicles = new VehicleGenerator().GenerateUniqueVehicles(1000, vehicleModels, warehouse).ToList();

            var model = new Model(vehicleModels, vehicles, cityMap, goods);

            File.WriteAllText(DataPath + "model.json", JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All
            }));
        }
    }
}
