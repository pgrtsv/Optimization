using System;
using System.Reactive.Disposables;
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
                this.OneWayBind(ViewModel, x => x.SimulationService, x => x.Map.SimulationService)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, x => x.SelectedObject, x => x.Map.SelectedObject);
                this.OneWayBind(ViewModel, x => x.Goods, x => x.GoodsDataGrid.Items)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.VehicleModels, x => x.VehicleModelsDataGrid.Items)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, x => x.SimulationService.TimeModifier, x => x.SimulationTimeComboBox.SelectedItem)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.StartSimulationCommand, x => x.StartSimulationMenuItem)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.StopSimulationCommand, x => x.StopSimulationMenuItem)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.SimulationService.CurrentDateTime,
                    x => x.SimulationDateTimeTextBlock.Text, x => x.ToString("f"))
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.SelectedObject, x => x.AdditionalDataContentControl.Content)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.SelectedObject, x => x.AdditionalDataTextBlock.Text,
                        x => x?.ToString())
                    .DisposeWith(disposables);
                this.WhenAnyValue(x => x.ViewModel.SimulationService.Profit,
                        x => x.ViewModel.SimulationService.LastProfit)
                    .Subscribe(x => ProfitTextBlock.Text = $"Доход: {x.Item1} ({x.Item2})")
                    .DisposeWith(disposables);
                this.WhenAnyValue(x => x.ViewModel.SimulationService.Leasing,
                        x => x.ViewModel.SimulationService.LastLeasing)
                    .Subscribe(x => LeasingTextBlock.Text = $"Аренда: {x.Item1} ({x.Item2})")
                    .DisposeWith(disposables);
                this.WhenAnyValue(x => x.ViewModel.SimulationService.Penalty,
                        x => x.ViewModel.SimulationService.LastPenalty)
                    .Subscribe(x => PenaltyTextBlock.Text = $"Штраф: {x.Item1} ({x.Item2})")
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.SimulationService.Result, x => x.ResultTextBlock.Text,
                        x => $"Результат: {x}")
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.SimulationService.AvailableVehicles, x => x.LeasingDataGrid.Items)
                    .DisposeWith(disposables);

            });
        }

        public Map Map => this.FindControl<Map>("Map");
        public DataGrid GoodsDataGrid => this.FindControl<DataGrid>(nameof(GoodsDataGrid));
        public DataGrid VehicleModelsDataGrid => this.FindControl<DataGrid>(nameof(VehicleModelsDataGrid));
        public ComboBox SimulationTimeComboBox => this.FindControl<ComboBox>(nameof(SimulationTimeComboBox));
        public DataGrid LeasingDataGrid => this.FindControl<DataGrid>(nameof(LeasingDataGrid));
        public TextBlock SimulationDateTimeTextBlock => this.FindControl<TextBlock>(nameof(SimulationDateTimeTextBlock));
        public TextBlock ProfitTextBlock => this.FindControl<TextBlock>(nameof(ProfitTextBlock));
        public TextBlock LeasingTextBlock => this.FindControl<TextBlock>(nameof(LeasingTextBlock));
        public TextBlock PenaltyTextBlock => this.FindControl<TextBlock>(nameof(PenaltyTextBlock));
        public TextBlock ResultTextBlock => this.FindControl<TextBlock>(nameof(ResultTextBlock));
        public MenuItem StartSimulationMenuItem => this.FindControl<MenuItem>(nameof(StartSimulationMenuItem));
        public MenuItem StopSimulationMenuItem => this.FindControl<MenuItem>(nameof(StopSimulationMenuItem));

        public ContentControl AdditionalDataContentControl =>
            this.FindControl<ContentControl>(nameof(AdditionalDataContentControl));

        public TextBlock AdditionalDataTextBlock =>
            this.FindControl<TextBlock>(nameof(AdditionalDataTextBlock));

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}