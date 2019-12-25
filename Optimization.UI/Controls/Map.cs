using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Optimization.Core;
using ReactiveUI;

namespace Optimization.UI.Controls
{
    public class Map: Canvas
    {
        public static readonly AvaloniaProperty<ReadOnlyObservableCollection<ICityPlace>> PlacesProperty =
            AvaloniaProperty.RegisterDirect<Map, ReadOnlyObservableCollection<ICityPlace>>(
                nameof(Places),
                x => x.Places,
                (x, value) => x.Places = value);

        private ReadOnlyObservableCollection<ICityPlace> _places;

        public Map()
        {
            DrawCommand = ReactiveCommand.Create(() =>
            {
                Children.Clear();
                foreach (var cityPlace in Places)
                {
                    Children.Add(CreateElementForPlace(cityPlace));
                }
            });
            this.WhenAnyValue(x => x.Places)
                .InvokeCommand(DrawCommand);
        }

        public ReadOnlyObservableCollection<ICityPlace> Places
        {
            get => _places;
            set => SetAndRaise(PlacesProperty, ref _places, value);
        }

        public ICommand DrawCommand { get; }


        public Control CreateElementForPlace(ICityPlace cityPlace)
        {
            var ellipse = new Ellipse {Width = 10, Height = 10, Fill = Brushes.Red};
            SetLeft(ellipse, cityPlace.Coordinates.X);
            SetTop(ellipse, cityPlace.Coordinates.Y);
            return ellipse;
        }
    }
}