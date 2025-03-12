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
        GenerateLagrangeInterpolationCommand = new RelayCommand<object>(_ => GenerateInterpolation(InterpolationCurve.GenerateLagrangeInterpolation));
        GenerateNewtonInterpolationCommand = new RelayCommand<object>(_ => GenerateInterpolation(InterpolationCurve.GenerateNewtonInterpolation));
    }

    private void AddPoint(Point point)
    {
        if (Points.Any() && point.X < Points.Last().X) return;

        Points.Add(point);
        InterpolationCurve.Points.Add(point);

        OnPropertyChanged(nameof(Points));
    }

    public void MovePoint(int index, Point newPosition)
    {
        if (index < 0 || index >= Points.Count) return;

        var leftLimit = index > 0 ? Points[index - 1].X : double.MinValue;
        var rightLimit = index < Points.Count - 1 ? Points[index + 1].X : double.MaxValue;

        // Keep the X-axis value within the range [leftLimit, rightLimit] without exceeding its neighboring points
        var clampedX = Math.Max(leftLimit, Math.Min(newPosition.X, rightLimit));

        var updatedPoint = new Point(clampedX, newPosition.Y);
        if (Points[index] == updatedPoint) return;
        Points[index] = updatedPoint;
        InterpolationCurve.Points[index] = updatedPoint;

        OnPropertyChanged(nameof(Points));
        GenerateInterpolation(InterpolationCurve.GenerateNewtonInterpolation);
    }

    private void GenerateInterpolation(Func<double, double> interpolationMethod)
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