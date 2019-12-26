using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using DynamicData;
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

        public static readonly AvaloniaProperty<SimulationService> SimulationServiceProperty =
            AvaloniaProperty.RegisterDirect<Map, SimulationService>(
                nameof(SimulationService),
                x => x.SimulationService,
                (x, value) => x.SimulationService = value);

        private ICityMap _cityMap;

        private SimulationService _simulationService;

        public Map()
        {
            DrawCommand = ReactiveCommand.Create(() =>
            {
                Children.Clear();
                if (CityMap == null) return;
                foreach (var cityRoad in CityMap.Roads) Children.Add(CreateElementForRoad(cityRoad));
                foreach (var cityPlace in CityMap.Places)
                {
                    Children.Add(CreateElementForPlace(cityPlace));
                    Children.Add(LabelPlace(cityPlace));
                }
                RenderTransform = new TransformGroup
                {
                    Children = new Transforms { new TranslateTransform(500, 500), new ScaleTransform(0.5, 0.5) }
                };
            });
            this.WhenAnyValue(x => x.CityMap)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => DrawCommand.Execute(null));
            this.WhenAnyValue(x => x.SimulationService)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    if (x == null) return;
                    RecolorRoads();
                    x.Interval
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(_ => RecolorRoads());
                });
        }

        public SimulationService SimulationService
        {
            get => _simulationService;
            set => SetAndRaise(SimulationServiceProperty, ref _simulationService, value);
        }

        public ICityMap CityMap
        {
            get => _cityMap;
            set => SetAndRaise(CityMapProperty, ref _cityMap, value);
        }

        public ICommand DrawCommand { get; }

        public void RecolorRoads()
        {
            if (SimulationService == null) return;
            foreach (var child in Children.OfType<Line>())
            {
                var usage = SimulationService.RoadsUsage.First(x => x.Road.Equals(child.DataContext)).Usage;
                child.Stroke = GetBrushFromRoadUsage(usage);
            }
        }


        public Ellipse CreateElementForPlace(ICityPlace cityPlace)
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

        public TextBlock LabelPlace(ICityPlace cityPlace)
        {
            var textBlock = new TextBlock();
            textBlock.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200), 0.3);
            textBlock.ZIndex = 2;
            SetLeft(textBlock, cityPlace.Coordinates.X);
            SetTop(textBlock, cityPlace.Coordinates.Y);

            switch (cityPlace)
            {
                case IWarehouse warehouse:
                    textBlock.Text = "Склад";
                    break;
                case ISalePoint salePoint:
                    textBlock.Text = "Точка продаж";
                    break;
            }

            return textBlock;
        }

        private ISolidColorBrush GetBrushFromRoadUsage(RoadUsage roadUsage)
        {
            return roadUsage switch
            {
                RoadUsage.High => Brushes.Red,
                RoadUsage.Medium => Brushes.Yellow,
                RoadUsage.Low => Brushes.Green
            };
        }

        public Line CreateElementForRoad(ICityRoad cityRoad)
        {
            var line = new Line
            {
                StartPoint = new Point(cityRoad.FirstPlace.Coordinates.X, cityRoad.FirstPlace.Coordinates.Y),
                EndPoint = new Point(cityRoad.SecondPlace.Coordinates.X, cityRoad.SecondPlace.Coordinates.Y),
                ZIndex = 1,
                StrokeThickness = 3,
                DataContext = cityRoad,
                Stroke = Brushes.Black
            };

            return line;
        }
    }
}