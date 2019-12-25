using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Optimization.UI.Controls;
using Optimization.UI.ViewModels;
using ReactiveUI;

namespace Optimization.UI.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public Map Map => this.FindControl<Map>("Map");
        public ListBox GoodsListBox => this.FindControl<ListBox>(nameof(GoodsListBox));
        public ListBox VehicleModelsListBox => this.FindControl<ListBox>(nameof(VehicleModelsListBox));
        public ListBox VehiclesListBox => this.FindControl<ListBox>(nameof(VehiclesListBox));
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
                this.OneWayBind(ViewModel, x => x.Goods, x => x.GoodsListBox.Items);
                this.OneWayBind(ViewModel, x => x.VehicleModels, x => x.VehicleModelsListBox.Items);
                this.OneWayBind(ViewModel, x => x.Vehicles, x => x.VehiclesListBox.Items);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
