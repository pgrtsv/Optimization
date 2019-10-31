using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.DailyModel;
using Optimization.Infrastructure;

namespace Optimization.DataGeneration
{
    /// <summary>
    /// Класс-генератор транспортных средств.
    /// </summary>
    public static class VehicleGenerator
    {
        private static Random _random = new Random();

        public static IEnumerable<Vehicle> GenerateUniqueVehicles(int count, IList<VehicleModel> vehicleModels)
        {
            if(count <= 0) throw new ArgumentException();

            for (int i = 0; i < count; i++)
            {
                var vehicleModel = _random.GetRandomFrom(vehicleModels, 1).First();
                yield return new Vehicle(i, vehicleModel);
            }
        }
    }
}