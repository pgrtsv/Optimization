using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Optimization.UI.Controls;
using Optimization.UI.ViewModels;
using ReactiveUI;

namespace Optimization.UI.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public Map Map => this.FindControl<Map>("Map");
        public DataGrid GoodsDataGrid => this.FindControl<DataGrid>(nameof(GoodsDataGrid));
        public DataGrid VehicleModelsDataGrid => this.FindControl<DataGrid>(nameof(VehicleModelsDataGrid));
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            ViewModel = (MainWindowViewModel) DataContext;
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.CityMap, x => x.Map.CityMap);
                this.OneWayBind(ViewModel, x => x.Goods, x => x.GoodsDataGrid.Items);
                this.OneWayBind(ViewModel, x => x.VehicleModels, x => x.VehicleModelsDataGrid.Items);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
