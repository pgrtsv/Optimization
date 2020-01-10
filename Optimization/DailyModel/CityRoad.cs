using System;
using FluentValidation;
using Optimization.Core;
using Optimization.Validation;

namespace Optimization.DailyModel
{
    public class CityRoad: ICityRoad
    {
        public CityRoad(ICityPlace firstPlace, ICityPlace secondPlace, RoadRank rank)
        {
            FirstPlace = firstPlace;
            SecondPlace = secondPlace;
            Rank = rank;
            CityRoadValidator.Instance.ValidateAndThrow(this);
        }

        public ICityPlace FirstPlace { get; }
        public ICityPlace SecondPlace { get; }
        public double GetDistance() => FirstPlace.Coordinates.DistanceTo(SecondPlace.Coordinates);
        public RoadRank Rank { get; }
        public RoadUsage Usage { get; private set; }

        public double Weight => throw new NotImplementedException();

        /*
        * Если ночь то все дороги пусты
        * Если рабочий день и время с 7 до 9 и с 17 до 19, то дороги на 1 более загружены
        * Если выходной день и время с 13 до 19 то дороги на  1 более загружены
        * В остальных случаях возвращается стандартное значение
        */
        public void GenerateRoadUsage(DateTime dateTime)
        {
            if (dateTime.Hour >= 21 || dateTime.Hour <= 4)
            {
                Usage = RoadUsage.Low;
                return;
            }
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    if (dateTime.Hour >= 7 && dateTime.Hour <= 9 || dateTime.Hour >= 17 && dateTime.Hour <= 19)
                    {
                        if (Rank == RoadRank.Low)
                            Usage = RoadUsage.Medium;
                        else if (Rank == RoadRank.Medium)
                            Usage = RoadUsage.High;
                        else
                            Usage = RoadUsage.High;
                        return;
                    }
                    break;
                default:
                    if (dateTime.Hour >= 13 && dateTime.Hour <= 19)
                    {
                        if (Rank == RoadRank.Low)
                            Usage = RoadUsage.Medium;
                        else if (Rank == RoadRank.Medium)
                            Usage = RoadUsage.High;
                        else
                            Usage = RoadUsage.High;
                        return;
                    }
                    break;
            }
            Usage = (RoadUsage)Rank;
        }
    }
}