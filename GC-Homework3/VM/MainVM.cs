using GC_Homework3.Commands;
using GC_Homework3.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GC_Homework3.VM;
public class MainVM : BaseVM
{
    public ObservableCollection<Point> Points { get; set; } = new();
    public PathGeometry InterpolatedPath { get; set; } = new();

    public ICommand AddPointCommand { get; set; }
    public ICommand GenerateLagrangeInterpolationCommand { get; set; }
    public ICommand GenerateNewtonInterpolationCommand { get; set; }

    public MainVM()
    {
        AddPointCommand = new RelayCommand<Point>(AddPoint);
        GenerateLagrangeInterpolationCommand = new RelayCommand<Point>(p => GenerateInterpolation(p, InterpolationCurve.GenerateLagrangeInterpolation));
        GenerateNewtonInterpolationCommand = new RelayCommand<Point>(p => GenerateInterpolation(p, InterpolationCurve.GenerateNewtonInterpolation));
    }

    private void AddPoint(Point point)
    {
        if (Points.Any() && point.X < Points.Last().X) return;

        Points.Add(point);
        InterpolationCurve.Points.Add(point);

        OnPropertyChanged(nameof(Points));
    }

    private void GenerateInterpolation(Point point, Func<double, double> interpolationMethod)
    {
        if (Points.Count < 2) return;

        var minX = Points.Min(p => p.X);
        var maxX = Points.Max(p => p.X);
        var step = (maxX - minX) / 500.0;

        var pathFigure = new PathFigure { StartPoint = new Point(minX, interpolationMethod(minX)), IsClosed = false };
        var bezierSegment = new PolyBezierSegment();

        for (double x = minX; x < maxX; x += step)
        {
            double y = interpolationMethod(x);
            bezierSegment.Points.Add(new Point(x, y));
        }
        pathFigure.Segments.Add(bezierSegment);
        InterpolatedPath = new PathGeometry([pathFigure]);

        OnPropertyChanged(nameof(InterpolatedPath));
    }
}