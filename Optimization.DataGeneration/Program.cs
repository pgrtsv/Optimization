using System;
using System.IO;
using Newtonsoft.Json;
using Optimization.DailyModel;

namespace Optimization.DataGeneration
{
    class Program
    {
        private static string DataPath = @"..\..\..\..\Data\";

        static void Main(string[] args)
        {
            using (var writer = File.CreateText(DataPath + "warehouse.json"))
            {
                writer.Write(JsonConvert.SerializeObject(GenerateWarehouse()));
            }
        }

        static Warehouse GenerateWarehouse()
        {
            return new Warehouse((0, 0));
        }
    }
}
