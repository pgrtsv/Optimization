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

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            ViewModel = (MainWindowViewModel) DataContext;
            this.WhenActivated(disposables => { this.OneWayBind(ViewModel, x => x.CityMap, x => x.Map.CityMap); });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
