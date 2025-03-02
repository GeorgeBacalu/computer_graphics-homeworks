using GC_Homework2.Commands;
using GC_Homework2.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GC_Homework2.VM;
public class MainVM : BaseVM
{
    private Curve curve = new();
    private double _canvasWidth = 800;
    private double _canvasHeight = 800;
    private const double _axesHorizontalPadding = 500, _axesVerticalPadding = 50;

    public double CanvasWidth { get => _canvasWidth; set { _canvasWidth = value; OnPropertyChanged(nameof(CanvasWidth)); } }
    public double CanvasHeight { get => _canvasHeight; set { _canvasHeight = value; OnPropertyChanged(nameof(CanvasHeight)); } }

    public ObservableCollection<Point> Points { get; } = new();
    public ObservableCollection<Line> Axes { get; set; } = new();
    public PathGeometry CurvePath { get; } = new();

    public ICommand GenerateCurveCommand { get; set; }
    public ICommand ApplyTransformationsCommand { get; set; }

    public MainVM()
    {
        GenerateCurveCommand = new RelayCommand<string>(GenerateCurve);
        ApplyTransformationsCommand = new RelayCommand<object>(ApplyTransformations);
    }

    private void GenerateCurve(string curveType)
    {
        Points.Clear();
        Axes.Clear();

        var generatedPoints = curveType switch
        {
            "CurveType1" => curve.Generate(u => new Point(Math.Cos(u), 2 * Math.Sin(u)), 0, 2 * Math.PI, 0.1, CanvasWidth, CanvasHeight),
            "CurveType2" => curve.Generate(u => new Point(u * Math.Cos(u), u * Math.Sin(u)), 0, 20, 0.1, CanvasWidth, CanvasHeight),
            "CurveType3" => curve.Generate(u => new Point(u, u * u + 1), -2, 2, 0.1, CanvasWidth, CanvasHeight),
            _ => new()
        };
        generatedPoints.ForEach(Points.Add);

        Axes.Add(new() { X1 = _axesHorizontalPadding, Y1 = CanvasHeight / 2, X2 = CanvasWidth - _axesHorizontalPadding, Y2 = CanvasHeight / 2, Stroke = Brushes.Black, StrokeThickness = 2 });
        Axes.Add(new() { X1 = CanvasWidth / 2, Y1 = _axesVerticalPadding, X2 = CanvasWidth / 2, Y2 = CanvasHeight - _axesVerticalPadding, Stroke = Brushes.Black, StrokeThickness = 2 });

        UpdateCurvePath();
        OnPropertyChanged(nameof(Points));
        OnPropertyChanged(nameof(Axes));
        OnPropertyChanged(nameof(CurvePath));
    }

    private void ApplyTransformations(object obj)
    {
        if (!Points.Any() || !Axes.Any()) return;

        var transformationResult = curve.ApplyTransformations(Points.ToList(), CanvasWidth, CanvasHeight);
        Points.Clear();
        Axes.Clear();
        transformationResult.Points.ForEach(Points.Add);
        transformationResult.Axes.ForEach(Axes.Add);

        UpdateCurvePath();
        OnPropertyChanged(nameof(Points));
        OnPropertyChanged(nameof(Axes));
        OnPropertyChanged(nameof(CurvePath));
    }

    private void UpdateCurvePath()
    {
        if (Points.Count > 1)
        {
            var pathFigure = new PathFigure { StartPoint = Points[0] };
            var bezierSegment = new PolyBezierSegment(Points.Skip(1), true);
            pathFigure.Segments.Add(bezierSegment);

            CurvePath.Figures.Clear();
            CurvePath.Figures.Add(pathFigure);
        }
    }
}