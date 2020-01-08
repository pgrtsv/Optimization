using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Optimization.Core;
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
        private readonly IDisposable _cleanUp;
        private double _timeModifier;

        public SimulationService(ICityMap cityMap)
        {
            TimeModifier = 60;
            CurrentDateTime = DateTime.Now;
            RoadsUsage = cityMap.Roads.Select(x => new CityRoadUsage(x, GenerateRoadUsage(CurrentDateTime, x))).ToList();

            var roadUsageBinding = this.WhenAnyValue(x => x.CurrentDateTime)
                .Subscribe(dateTime =>
                {
                    foreach (var roadUsage in RoadsUsage)
                        roadUsage.Usage = GenerateRoadUsage(dateTime, roadUsage.Road);
                });
            Interval = Observable.Generate(
                this,
                x => x.IsRunning,
                x =>
                {
                    CurrentDateTime += TimeSpan.FromMinutes(1);
                    return x;
                },
                x => x.CurrentDateTime,
                x => TimeSpan.FromMinutes(1.0 / TimeModifier),
                RxApp.MainThreadScheduler);
            _cleanUp = Disposable.Create(() => { roadUsageBinding.Dispose(); });
        }

        [Reactive] public bool IsRunning { get; private set; }

        public IObservable<DateTime> Interval { get; }

        [Reactive]
        public double TimeModifier
        {
            get => _timeModifier;
            set
            {
                if (value < 1)
                    throw new ArgumentException("Замедление времени не поддерживается.");
                _timeModifier = value;
            }
        }

        [Reactive] public DateTime CurrentDateTime { get; private set; }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }

        public IReadOnlyCollection<CityRoadUsage> RoadsUsage { get; }

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public static RoadUsage GenerateRoadUsage(DateTime dateTime, ICityRoad road)
        {
            return (RoadUsage) new Random().Next(0, 3);
        }
    }
}