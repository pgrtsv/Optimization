using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Optimization.Core;
using Optimization.DailyModel;
using ReactiveUI;

namespace Optimization.UI.Controls
{
    public class Map : Canvas
    {
        public static readonly AvaloniaProperty<SimulationService> SimulationServiceProperty =
            AvaloniaProperty.RegisterDirect<Map, SimulationService>(
                nameof(SimulationService),
                x => x.SimulationService,
                (x, value) => x.SimulationService = value);

        public static readonly AvaloniaProperty<object> SelectedObjectProperty =
            AvaloniaProperty.RegisterDirect<Map, object>(
                nameof(SelectedObject),
                x => x.SelectedObject,
                (x, value) => x.SelectedObject = value);

        private SimulationService _simulationService;
        private object _selectedObject;

        private readonly ReactiveCommand<object, Unit> _selectObjectCommand;

        public Map()
        {
            _selectObjectCommand = ReactiveCommand.Create<object>(x => { SelectedObject = x; });
            DrawCommand = ReactiveCommand.Create(() =>
            {
                Children.Clear();
                if (SimulationService == null) return;
                foreach (var cityRoad in SimulationService.CityMap.Roads) Children.Add(CreateElementForRoad(cityRoad));
                foreach (var cityPlace in SimulationService.CityMap.Places)
                {
                    Children.Add(CreateElementForPlace(cityPlace));
                    Children.Add(LabelPlace(cityPlace));
                }

                RenderTransform = new TransformGroup
                {
                    Children = new Transforms {new TranslateTransform(500, 500), new ScaleTransform(0.5, 0.5)}
                };
            });
            this.WhenAnyValue(x => x.SimulationService)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    if (x == null) return;
                    DrawCommand.Execute(null);
                    RecolorRoads();
                    x.MinuteInterval
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(_ =>
                        {
                            RecolorRoads();
                            MoveVehicles();
                        });
                    x.DayInterval
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(_ =>
                        {
                            foreach (var vehicleView in Children.OfType<Button>().Where(x => x.DataContext is IVehicle))
                                Children.Remove(vehicleView);

                            foreach (var vehicle in SimulationService.Solutions.Select(x => x.Vehicle))
                                Children.Add(CreateElementForVehicle(vehicle));
                        });
                });
        }

        public SimulationService SimulationService
        {
            get => _simulationService;
            set => SetAndRaise(SimulationServiceProperty, ref _simulationService, value);
        }

        public object SelectedObject
        {
            get => _selectedObject;
            set => SetAndRaise(SelectedObjectProperty, ref _selectedObject, value);
        }

        public ICommand DrawCommand { get; }

        public void RecolorRoads()
        {
            if (SimulationService == null) return;
            foreach (var child in Children.OfType<Line>())
            {
                var usage = SimulationService.CityMap.Roads.First(x => x.Equals(child.DataContext)).Usage;

                child.Stroke = GetBrushFromRoadUsage(usage);
            }
        }

        public void MoveVehicles()
        {
            if (SimulationService == null) return;
            foreach (var vehicleView in Children.OfType<Button>().Where(x => x.DataContext is IVehicle))
            {
                SetTop(vehicleView, ((IVehicle)DataContext).Position.Y);
                SetLeft(vehicleView, ((IVehicle)DataContext).Position.X);
            }
        }


        public Button CreateElementForPlace(ICityPlace cityPlace)
        {
            var ellipse = new Button
            {
                Width = 10, Height = 10, ZIndex = 2,
                DataContext = cityPlace,
                Command = _selectObjectCommand,
                CommandParameter = cityPlace,
                Background = GetBrushForPlace(cityPlace),
                BorderBrush = GetBrushForPlace(cityPlace),
            };
            SetLeft(ellipse, cityPlace.Coordinates.X - 5);
            SetTop(ellipse, cityPlace.Coordinates.Y - 5);
            return ellipse;
        }

        private ISolidColorBrush GetBrushForPlace(ICityPlace cityPlace)
        {
            return cityPlace switch
                {
                Warehouse _ => Brushes.Blue,
                SalePoint _ => Brushes.Brown,
                _ => Brushes.Black
                };
        }

        public Button CreateElementForVehicle(IVehicle vehicle)
        {
            var button = new Button
            {
                Width = 20,
                Height = 20,
                ZIndex = 3,
                Background = GetBrushForVehicle(vehicle),
                DataContext = vehicle,
                Command = _selectObjectCommand,
                CommandParameter = vehicle,
                BorderBrush = GetBrushForVehicle(vehicle)
            };
            SetTop(button, vehicle.Position.Y);
            SetLeft(button, vehicle.Position.X);
            return button;
        }

        private ISolidColorBrush GetBrushForVehicle(IVehicle vehicle)
        {
            return vehicle.VehicleModel.Type switch
                {
                VehicleType.BigTruck => Brushes.Khaki,
                VehicleType.SmallTruck => Brushes.Lavender,
                VehicleType.Passenger => Brushes.Goldenrod,
                };
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