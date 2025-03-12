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
    private double _canvasWidth = 800;
    private double _canvasHeight = 800;
    private string _lastCurveType = "";
    private const double PointsCount = 150;

    public double CanvasWidth { get => _canvasWidth; set { _canvasWidth = value; OnPropertyChanged(nameof(CanvasWidth)); } }
    public double CanvasHeight { get => _canvasHeight; set { _canvasHeight = value; OnPropertyChanged(nameof(CanvasHeight)); } }

    public ObservableCollection<Point> Points { get; } = new();
    public ObservableCollection<Line> Axes { get; set; } = new();
    public PathGeometry CurvePath { get; } = new();

    public ICommand GenerateCurveTransformCommand { get; set; }

    public MainVM() => GenerateCurveTransformCommand = new RelayCommand<string>(GenerateCurveTransform);

    private void GenerateCurveTransform(string curveType)
    {
        _lastCurveType = curveType;
        RegenerateCurve();
    }

    public void RegenerateCurve()
    {
        if (string.IsNullOrWhiteSpace(_lastCurveType)) return;

        var generatedPoints = _lastCurveType switch
        {
            "CurveType1" => Curve.Generate(u => new(Math.Cos(u), 2 * Math.Sin(u)), 0, 2 * Math.PI, PointsCount, CanvasWidth, CanvasHeight),
            "CurveType2" => Curve.Generate(u => new(u * Math.Cos(u), u * Math.Sin(u)), 0, 20, PointsCount, CanvasWidth, CanvasHeight),
            "CurveType3" => Curve.Generate(u => new(u, u * u + 1), -2, 2, PointsCount, CanvasWidth, CanvasHeight),
            _ => new()
        };
        var transformationResult = Curve.ApplyTransformations(generatedPoints.ToList(), CanvasWidth, CanvasHeight);

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