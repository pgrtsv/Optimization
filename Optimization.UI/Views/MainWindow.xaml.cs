using System;
using System.Reactive.Linq;
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
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            ViewModel = (MainWindowViewModel) DataContext;
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.SimulationService, x => x.Map.SimulationService);
                this.OneWayBind(ViewModel, x => x.Goods, x => x.GoodsDataGrid.Items);
                this.OneWayBind(ViewModel, x => x.VehicleModels, x => x.VehicleModelsDataGrid.Items);
                this.Bind(ViewModel, x => x.SimulationService.TimeModifier, x => x.SimulationTimeComboBox.SelectedItem);
                this.BindCommand(ViewModel, x => x.StartSimulationCommand, x => x.StartSimulationMenuItem);
                this.BindCommand(ViewModel, x => x.StopSimulationCommand, x => x.StopSimulationMenuItem);
                this.WhenAnyValue(x => x.ViewModel.SimulationService.CurrentDateTime)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(x => SimulationDateTimeTextBlock.Text = x.ToString("f"));
            });
        }

        public Map Map => this.FindControl<Map>("Map");
        public DataGrid GoodsDataGrid => this.FindControl<DataGrid>(nameof(GoodsDataGrid));
        public DataGrid VehicleModelsDataGrid => this.FindControl<DataGrid>(nameof(VehicleModelsDataGrid));
        public ComboBox SimulationTimeComboBox => this.FindControl<ComboBox>(nameof(SimulationTimeComboBox));

        public TextBlock SimulationDateTimeTextBlock => this.FindControl<TextBlock>(nameof(SimulationDateTimeTextBlock));
        public MenuItem StartSimulationMenuItem => this.FindControl<MenuItem>(nameof(StartSimulationMenuItem));
        public MenuItem StopSimulationMenuItem => this.FindControl<MenuItem>(nameof(StopSimulationMenuItem));

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}