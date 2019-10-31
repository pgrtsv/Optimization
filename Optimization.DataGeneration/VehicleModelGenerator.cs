using System;
using System.Collections.Generic;
using Optimization.DailyModel;
using Optimization.Infrastructure;
using Optimization.Interfaces;

namespace Optimization.DataGeneration
{
    /// <summary>
    /// Класс-генератор моделей транспортных средств.
    /// </summary>
    public static class VehicleModelGenerator
    {
        private static Random _random = new Random();

        /// <summary>
        /// Генерирует перечисление уникальных моделей машин.
        /// </summary>
        /// <param name="count">Количество генерируемых экземпляров.</param>
        /// <returns>Перечисление уникальных моделей машин.</returns>
        public static IEnumerable<VehicleModel> GenerateUniqueVehicleModels(int count)
        {
            for (int i = 0; i < count; i++)
                yield return GenerateVehicleModel();
        }

        /// <summary>
        /// Генерирует модель транспортного средства.
        /// </summary>
        /// <returns>Экземпляр <see cref="VehicleModel"/></returns>
        private static VehicleModel GenerateVehicleModel()
        {
            VehicleType vehicleType = (VehicleType) _random.Next(0, 2);
            var dimension = GenerateDimensions(vehicleType);
            var capacity = GenerateCapacity(vehicleType, dimension);
            var accelerationTime = GenerateAccelerationTime(vehicleType, dimension);
            var price = GeneratePrice(vehicleType, capacity, accelerationTime);
            return new VehicleModel(capacity, accelerationTime, dimension, vehicleType, price);
        }

        /// <summary>
        /// Генерирует габариты транспорта.
        /// </summary>
        /// <param name="vehicleType">Тип транспорта.</param>
        /// <returns>Габариты транспорта (ДxШxВ - в метрах).</returns>
        private static (double, double, double) GenerateDimensions(VehicleType vehicleType)
        {
            double length, width, height;
            switch (vehicleType)
            {
                case VehicleType.BigTruck:
                    length = GenerateAndRound(8, 16);
                    width = GenerateAndRound(2.4, 2.7);
                    height = GenerateAndRound(2.5, 3.2);
                    return (length, width, height);
                case VehicleType.SmallTruck:
                    length = GenerateAndRound(5.3, 7);
                    width = GenerateAndRound(2.1, 2.5);
                    height = GenerateAndRound(1.8, 2.4);
                    return (length, width, height);
                case VehicleType.Passenger:
                    length = GenerateAndRound(3.8, 5.2);
                    width = GenerateAndRound(1.8, 2.1);
                    height = GenerateAndRound(1.2, 1.7);
                    return (length, width, height);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Генерирует вместительность транспорта.
        /// </summary>
        /// <param name="vehicleType">Тип транспорта.</param>
        /// <param name="dimensions">Габариты транспорта (ДxШxВ - в метрах).</param>
        /// <returns>Вместительность (объём груза, который можно загрузить в транспорт), м3.</returns>
        private static double GenerateCapacity(VehicleType vehicleType, (double, double, double) dimensions)
        {
            /* Размерности багажника/прицепа. */
            double width, height, length;
            switch (vehicleType)
            {
                case VehicleType.BigTruck:
                    length =  GenerateAndRound(dimensions.Item1 - 2.5, dimensions.Item1 - 1.5, 2);
                    width = dimensions.Item2 * GenerateAndRound(0.85, 0.95, 2);
                    height = dimensions.Item3 * GenerateAndRound(0.85, 0.95, 2);
                    break;
                case VehicleType.SmallTruck:
                    length = GenerateAndRound(dimensions.Item1 - 1.6, dimensions.Item1 - 1.3, 2);
                    width = dimensions.Item2 * GenerateAndRound(0.85, 0.95, 2);
                    height = dimensions.Item3 * GenerateAndRound(0.85, 0.95, 2);
                    break;
                case VehicleType.Passenger:
                    length = GenerateAndRound(dimensions.Item1 * 0.1, dimensions.Item1 * 0.2, 2);
                    width = dimensions.Item2 * GenerateAndRound(0.8, 0.95, 2);
                    height = dimensions.Item3 * GenerateAndRound(0.3, 0.5, 2);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return Math.Round(length * width * height, 2);
        }

        /// <summary>
        /// Генерирует время разгона транспорта до 100 км/ч.
        /// </summary>
        /// <param name="vehicleType">Тип транспорта.</param>
        /// <param name="dimensions">Габариты транспорта (ДxШxВ - в метрах).</param>
        /// <returns>Время разгона транспорта до 100 км/ч в секундах.</returns>
        private static double GenerateAccelerationTime(VehicleType vehicleType, (double, double, double) dimensions)
        {
            double accelerationTime;
            /* Далее начисляется штраф за габариты транспорта,
             * так что порог генерации - занижен. */
            switch (vehicleType)
            {
                case VehicleType.BigTruck:
                    accelerationTime = GenerateAndRound(14, 25);
                    break;
                case VehicleType.SmallTruck:
                    accelerationTime = GenerateAndRound(7, 14);
                    break;
                case VehicleType.Passenger:
                    accelerationTime = GenerateAndRound(5, 9);
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            /* Расчитываем штраф (доп. время) в зависимости от габаритов,
             * тем самым учитывая их. */
            var volume = dimensions.Item1 * dimensions.Item2 * dimensions.Item3;
            var dimensionPenalty = GenerateAndRound(0, volume * 0.35);

            return accelerationTime + dimensionPenalty;
        }

        /// <summary>
        /// Генерирует цену аренды транспорта (в рублях).
        /// </summary>
        /// <param name="vehicleType">Тип транспорта.</param>
        /// <param name="capacity">Вместительность (объём груза, который можно загрузить в транспорт), м3.</param>
        /// <param name="accelerationTime">Время разгона транспорта до 100 км/ч в секундах.</param>
        /// <returns>Цена аренды транспорта (в рублях) за сутки.</returns>
        private static double GeneratePrice(VehicleType vehicleType, double capacity, double accelerationTime)
        {
            /* Далее будут учтены габариты транспорта и время разгона,
             * так что порог генерации - занижен. */
            double price;
            switch (vehicleType)
            {
                case VehicleType.BigTruck:
                    price = GenerateAndRound(6400, 8000);
                    break;
                case VehicleType.SmallTruck:
                    price = _random.Next(1700, 2900);
                    break;
                case VehicleType.Passenger:
                    price = GenerateAndRound(800, 2100);
                    break;
                default:
                    throw new NotImplementedException();
            }

            /* Расчитываем штраф (доп. время) в зависимости от габаритов,
             * тем самым учитывая их. */
            var dimensionAddition = GenerateAndRound(capacity * 60, capacity * 100, 0);
            var accelerationAddition = GenerateAndRound(1000/accelerationTime, 5000/accelerationTime, 0);

            return price + dimensionAddition + accelerationAddition;
        }

        private static double GenerateAndRound(double min, double max, int precision = 1)
        {
            return Math.Round(_random.NextDouble(min, max), precision, MidpointRounding.AwayFromZero);
        }

    }
}
