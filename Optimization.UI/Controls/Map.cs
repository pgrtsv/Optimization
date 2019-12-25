using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Optimization.Core;
using Optimization.DailyModel;
using ReactiveUI;

namespace Optimization.UI.Controls
{
    public class Map : Canvas
    {
        public static readonly AvaloniaProperty<ICityMap> CityMapProperty =
            AvaloniaProperty.RegisterDirect<Map, ICityMap>(
                nameof(CityMap),
                x => x.CityMap,
                (x, value) => x.CityMap = value);

        private ICityMap _cityMap;

        public Map()
        {
            DrawCommand = ReactiveCommand.Create(() =>
            {
                Children.Clear();
                if (CityMap == null) return;
                foreach (var cityRoad in CityMap.Roads) Children.Add(CreateElementForRoad(cityRoad));
                foreach (var cityPlace in CityMap.Places) Children.Add(CreateElementForPlace(cityPlace));
                RenderTransform = new TransformGroup
                {
                    Children = new Transforms {new TranslateTransform(500, 500), new ScaleTransform(0.5, 0.5)}
                };
            });
            this.WhenAnyValue(x => x.CityMap)
                .Subscribe(x => DrawCommand.Execute(null));
        }

        public ICityMap CityMap
        {
            get => _cityMap;
            set => SetAndRaise(CityMapProperty, ref _cityMap, value);
        }

        public ICommand DrawCommand { get; }


        public Control CreateElementForPlace(ICityPlace cityPlace)
        {
            var ellipse = new Ellipse {Width = 10, Height = 10, ZIndex = 2};
            if (cityPlace is Warehouse)
                ellipse.Fill = Brushes.Blue;
            else if (cityPlace is SalePoint)
                ellipse.Fill = Brushes.Brown;
            else
                ellipse.Fill = Brushes.Black;
            SetLeft(ellipse, cityPlace.Coordinates.X - 5);
            SetTop(ellipse, cityPlace.Coordinates.Y - 5);
            return ellipse;
        }

        public Control CreateElementForRoad(ICityRoad cityRoad)
        {
            var line = new Line
            {
                StartPoint = new Point(cityRoad.FirstPlace.Coordinates.X, cityRoad.FirstPlace.Coordinates.Y),
                EndPoint = new Point(cityRoad.SecondPlace.Coordinates.X, cityRoad.SecondPlace.Coordinates.Y),
                ZIndex = 1,
                StrokeThickness = 3
            };
            switch (cityRoad.Rank)
            {
                case RoadRank.High:
                    line.Stroke = Brushes.Red;
                    break;
                case RoadRank.Medium:
                    line.Stroke = Brushes.Yellow;
                    break;
                case RoadRank.Low:
                    line.Stroke = Brushes.Green;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return line;
        }
    }
}