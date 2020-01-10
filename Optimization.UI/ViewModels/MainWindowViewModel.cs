using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using Optimization.Core;
using Optimization.DailyModel;
using Optimization.DataGeneration;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Optimization.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private object _selectedObject;
        public CityMap CityMap { get; }

        public Goods Goods { get; }

        public ObservableCollection<VehicleModel> VehicleModels { get; }

        public ObservableCollection<Vehicle> Vehicles { get; }

        public SimulationService SimulationService { get; }
        public double[] AvailableSimulationTimes { get; }

        public MainWindowViewModel()
        {
            AvailableSimulationTimes = new []{1.0, 10, 60, 100, 600};

            Goods = new Goods(GoodGenerator.GenerateUniqueGoods(50).Cast<IGood>().ToList());
            CityMap = new CityMapGenerator().Generate(Goods);
            VehicleModels = new ObservableCollection<VehicleModel>(new VehicleModelGenerator().GenerateUniqueVehicleModels(20));
            Vehicles = new ObservableCollection<Vehicle>(new VehicleGenerator().GenerateUniqueVehicles(50, VehicleModels, CityMap.Places.OfType<IWarehouse>().First()));
            SimulationService = new SimulationService(CityMap, VehicleModels, new Optimizer(CityMap));
            var canStartSimulation = SimulationService.WhenAnyValue(x => x.IsRunning)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(x => !x);
            StartSimulationCommand = ReactiveCommand.Create(() =>
            {
                SimulationService.Start();
            }, canStartSimulation);

            var canStopSimulation = SimulationService.WhenAnyValue(x => x.IsRunning);
            StopSimulationCommand = ReactiveCommand.Create(() => SimulationService.Stop(), canStopSimulation);
        }

        public ReactiveCommand<Unit, Unit> StartSimulationCommand { get; }
        public ReactiveCommand<Unit, Unit> StopSimulationCommand { get; }

        public object SelectedObject
        {
            get => _selectedObject;
            set => this.RaiseAndSetIfChanged(ref _selectedObject, value);
        }
    }
}
