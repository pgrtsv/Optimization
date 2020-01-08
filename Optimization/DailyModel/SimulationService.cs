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
    public class CityRoadUsage : INotifyPropertyChanged
    {
        private RoadUsage _usage;

        public CityRoadUsage(ICityRoad road, RoadUsage usage)
        {
            Road = road;
            Usage = usage;
        }

        public ICityRoad Road { get; }

        public RoadUsage Usage
        {
            get => _usage;
            set
            {
                _usage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Usage)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class SimulationService : ReactiveObject, IDisposable
    {
        private readonly List<VehicleModel> _availableVehicleModels;
        private double _timeModifier;
        private readonly Subject<DateTime> _interval = new Subject<DateTime>();
        private IDisposable _currentTimer;

        public SimulationService(ICityMap cityMap, IEnumerable<VehicleModel> availableVehicleModels)
        {
            CityMap = cityMap;
            _availableVehicleModels = availableVehicleModels.ToList();
            TimeModifier = 60;
            CurrentDateTime = DateTime.Now;
        }

        public ICityMap CityMap { get; }

        public IReadOnlyCollection<IVehicleModel> AvailableVehicleModels => _availableVehicleModels;

        [Reactive] public bool IsRunning { get; private set; }

        public IObservable<DateTime> Interval => _interval;

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

        public IReadOnlyCollection<Vehicle> AvailableVehicles { get; private set; }

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
                // Ежедневные обновления.
                AvailableVehicles = new VehicleGenerator().GenerateUniqueVehicles(10, _availableVehicleModels, CityMap.Places.OfType<IWarehouse>().First())
                    .ToArray();

            //Ежеминутные обновления.
            foreach (var road in CityMap.Roads)
                road.GenerateRoadUsage(CurrentDateTime);

            // Следующая минута.
            CurrentDateTime += TimeSpan.FromMinutes(1);
            _interval.OnNext(CurrentDateTime);
        }

        public void Stop()
        {
            IsRunning = false;
            _currentTimer?.Dispose();
        }
    }
}