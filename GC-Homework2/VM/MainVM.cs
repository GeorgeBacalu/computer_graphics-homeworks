using GC_Homework2.Commands;
using GC_Homework2.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GC_Homework2.VM;
public class MainVM : BaseVM
{
    private Curve curve = new();
    private double _canvasWidth = 800;
    private double _canvasHeight = 800;

    public double CanvasWidth { get => _canvasWidth; set { _canvasWidth = value; OnPropertyChanged(nameof(CanvasWidth)); } }
    public double CanvasHeight { get => _canvasHeight; set { _canvasHeight = value; OnPropertyChanged(nameof(CanvasHeight)); } }

    public ObservableCollection<Point> Points { get; } = new();
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
        var generatedPoints = curveType switch
        {
            "CurveType1" => curve.Generate(u => new Point(Math.Cos(u), 2 * Math.Sin(u)), 0, 2 * Math.PI, 0.1, CanvasWidth, CanvasHeight),
            "CurveType2" => curve.Generate(u => new Point(u * Math.Cos(u), u * Math.Sin(u)), 0, 20, 0.1, CanvasWidth, CanvasHeight),
            "CurveType3" => curve.Generate(u => new Point(u, u * u + 1), -2, 2, 0.1, CanvasWidth, CanvasHeight),
            _ => new()
        };
        generatedPoints.ForEach(Points.Add);

        if (Points.Count > 1)
        {
            var pathFigure = new PathFigure { StartPoint = Points[0] };
            var bezierSegment = new PolyBezierSegment(Points.Skip(1), true);
            pathFigure.Segments.Add(bezierSegment);

            CurvePath.Figures.Clear();
            CurvePath.Figures.Add(pathFigure);
        }
        OnPropertyChanged(nameof(Points));
        OnPropertyChanged(nameof(CurvePath));
    }

    private void ApplyTransformations(object obj)
    {
        if (!Points.Any()) return;

        var transformedPoints = curve.ApplyTransformations(Points.ToList(), CanvasWidth, CanvasHeight);
        Points.Clear();
        transformedPoints.ForEach(Points.Add);

        if (Points.Count > 1)
        {
            var pathFigure = new PathFigure { StartPoint = Points[0] };
            var bezierSegment = new PolyBezierSegment(Points.Skip(1), true);
            pathFigure.Segments.Add(bezierSegment);

            CurvePath.Figures.Clear();
            CurvePath.Figures.Add(pathFigure);
        }
        OnPropertyChanged(nameof(Points));
        OnPropertyChanged(nameof(CurvePath));
    }
}