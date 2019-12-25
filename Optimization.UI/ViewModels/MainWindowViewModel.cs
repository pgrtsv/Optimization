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

        public MainWindowViewModel()
        {
            Goods = new Goods(GoodGenerator.GenerateUniqueGoods(50).Cast<IGood>().ToList());
            CityMap = new CityMapGenerator().Generate(Goods);
        }
    }
}
