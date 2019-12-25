using System.Collections.ObjectModel;
using System.Linq;
using Optimization.Core;
using Optimization.DailyModel;
using Optimization.DataGeneration;

namespace Optimization.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public CityMap CityMap { get; }

        public Goods Goods { get; }

        public ObservableCollection<VehicleModel> VehicleModels { get; }

        public ObservableCollection<Vehicle> Vehicles { get; }

        public MainWindowViewModel()
        {
            Goods = new Goods(GoodGenerator.GenerateUniqueGoods(50).Cast<IGood>().ToList());
            CityMap = new CityMapGenerator().Generate(Goods);
            VehicleModels = new ObservableCollection<VehicleModel>(new VehicleModelGenerator().GenerateUniqueVehicleModels(20));
            Vehicles = new ObservableCollection<Vehicle>(new VehicleGenerator().GenerateUniqueVehicles(50, VehicleModels));
        }
    }
}
