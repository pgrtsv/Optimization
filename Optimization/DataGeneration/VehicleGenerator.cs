using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Core;
using Optimization.DailyModel;
using Optimization.Infrastructure;

namespace Optimization.DataGeneration
{
    /// <summary>
    /// Генератор транспортных средств.
    /// </summary>
    public class VehicleGenerator
    {
        public IEnumerable<Vehicle> GenerateUniqueVehicles(int count, IList<VehicleModel> vehicleModels, IWarehouse warehouse)
        {
            if(count <= 0) throw new ArgumentException();

            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                var vehicleModel = random.GetRandomFrom(vehicleModels, 1).First();
                yield return new Vehicle(i, vehicleModel, warehouse);
            }
        }
    }
}