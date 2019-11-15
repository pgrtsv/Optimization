using Avalonia;
using Avalonia.Markup.Xaml;

namespace Optimization.UI
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
