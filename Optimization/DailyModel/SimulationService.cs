using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Optimization.Core;
using Optimization.DataGeneration;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Optimization.DailyModel
{
    public class SimulationService : ReactiveObject, IDisposable
    {
        private readonly IOptimizer _optimizer;
        private readonly List<VehicleModel> _availableVehicleModels;
        private double _timeModifier;
        private readonly Subject<DateTime> _dayInterval = new Subject<DateTime>();
        private readonly Subject<DateTime> _minuteInterval = new Subject<DateTime>();
        private IDisposable _currentTimer;

        public SimulationService(
            ICityMap cityMap, 
            IEnumerable<VehicleModel> availableVehicleModels,
            IOptimizer optimizer)
        {
            _optimizer = optimizer;
            CityMap = cityMap;
            _availableVehicleModels = availableVehicleModels.ToList();
            TimeModifier = 60;
            CurrentDateTime = DateTime.Now;
        }

        public ICityMap CityMap { get; }
        
        public IReadOnlyCollection<IVehicleModel> AvailableVehicleModels => _availableVehicleModels;

        [Reactive] public bool IsRunning { get; private set; }

        public IObservable<DateTime> MinuteInterval => _minuteInterval;

        public IObservable<DateTime> DayInterval => _dayInterval;

        public double TimeModifier
        {
            get => _timeModifier;
            set
            {
                if (value < 1)
                    throw new ArgumentException("Замедление времени не поддерживается.");
                _timeModifier = value;
                if (IsRunning)
                {
                    _currentTimer?.Dispose();
                    _currentTimer = Observable.Interval(TimeSpan.FromMilliseconds(1000.0 / TimeModifier), RxApp.MainThreadScheduler)
                        .Subscribe(_ => NextMinute());
                }
            }
        }

        [Reactive] public DateTime CurrentDateTime { get; private set; }

        [Reactive] public IReadOnlyCollection<Vehicle> AvailableVehicles { get; private set; } = new List<Vehicle>();
        [Reactive] public IReadOnlyCollection<IOrder> DailyOrders { get; private set; }
        [Reactive] public IReadOnlyCollection<IOptimizerSolution> Solutions { get; private set; }

        [Reactive] public decimal Penalty { get; private set; }
        [Reactive] public decimal Profit { get; private set; }

        public void Dispose()
        {
            _currentTimer?.Dispose();
        }

        public void Start()
        {
            IsRunning = true;
            _currentTimer = Observable.Interval(TimeSpan.FromMilliseconds(1000.0 / TimeModifier), RxApp.MainThreadScheduler)
                .Subscribe(_ => NextMinute());
        }

        private void NextMinute()
        {
            if (CurrentDateTime.TimeOfDay >= TimeSpan.FromHours(9) &&
                CurrentDateTime.TimeOfDay < TimeSpan.FromHours(9) + TimeSpan.FromMinutes(1))
            {
                // Ежедневный подсчёт штрафов и доходов.
                if (Solutions != null)
                    foreach (var solution in Solutions)
                        if (solution.Vehicle.Position.Equals(solution.Route.End.Coordinates))
                        {
                            foreach (var good in solution.Goods)
                                Profit += good.Key.Price * good.Value;
                        }
                        else
                        {
                            foreach (var good in solution.Goods)
                                Penalty += good.Key.Price * good.Value;
                        }

                // Ежедневные обновления.
                AvailableVehicles = new VehicleGenerator().GenerateUniqueVehicles(10, _availableVehicleModels,
                        CityMap.Places.OfType<IWarehouse>().First())
                    .ToArray();
                DailyOrders = CityMap.Places.OfType<SalePoint>().Select(x => x.GenerateOrder()).ToArray();

                Solutions = _optimizer.Solve(AvailableVehicles, DailyOrders, CityMap, CurrentDateTime);

                // Заносим в расходы стоимость аренды выбранных оптимизатором ТС.
                foreach (var vehicle in Solutions.Select(x => x.Vehicle))
                    Profit -= (decimal) vehicle.RentalPrice;

                _dayInterval.OnNext(CurrentDateTime);
            }

            //Ежеминутные обновления.
            foreach (var road in CityMap.Roads)
                road.GenerateRoadUsage(CurrentDateTime);
            if (Solutions != null)
                foreach (var vehicle in Solutions.Select(x => x.Vehicle))
                    vehicle.Move(TimeSpan.FromMinutes(1));

            // Следующая минута.
            CurrentDateTime += TimeSpan.FromMinutes(1);
            _minuteInterval.OnNext(CurrentDateTime);
        }

        public void Stop()
        {
            IsRunning = false;
            _currentTimer?.Dispose();
        }
    }
}