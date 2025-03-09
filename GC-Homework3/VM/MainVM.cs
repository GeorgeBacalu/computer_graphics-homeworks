using GC_Homework3.Commands;
using GC_Homework3.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GC_Homework3.VM;
public class MainVM : BaseVM
{
    private InterpolationCurve interpolationCurve = new();

    public ObservableCollection<Point> Points { get; set; } = new();
    public ObservableCollection<Point> InterpolatedPoints { get; set; } = new();
    public PointCollection InterpolatedCurve { get; set; } = new();

    public ICommand AddPointCommand { get; set; }
    public ICommand GenerateLagrangeInterpolationCommand { get; set; }
    public ICommand GenerateNewtonInterpolationCommand { get; set; }

    public MainVM()
    {
        AddPointCommand = new RelayCommand<Point>(AddPoint);
        GenerateLagrangeInterpolationCommand = new RelayCommand<Point>(GenerateLagrangeInterpolation);
        GenerateNewtonInterpolationCommand = new RelayCommand<Point>(GenerateNewtonInterpolation);
    }

    private void AddPoint(Point point)
    {
        if (Points.Any() && point.X < Points.Last().X) return;

        Points.Add(point);
        interpolationCurve.Points.Add(point);

        OnPropertyChanged(nameof(Points));
    }

    private void GenerateLagrangeInterpolation(Point point)
    {
        if (Points.Count < 2) return;

        InterpolatedPoints.Clear();
        InterpolatedCurve.Clear();

        var minX = Points.Min(p => p.X);
        var maxX = Points.Max(p => p.X);
        var step = (maxX - minX) / 500.0;

        for (double x = minX; x < maxX; x += step)
        {
            double y = interpolationCurve.GenerateLagrangeInterpolation(x);
            var newPoint = new Point(x, y);
            InterpolatedPoints.Add(newPoint);
        }
        InterpolatedCurve = new PointCollection(InterpolatedPoints);
        OnPropertyChanged(nameof(InterpolatedPoints));
        OnPropertyChanged(nameof(InterpolatedCurve));
    }

    private void GenerateNewtonInterpolation(Point point)
    {
        if (Points.Count < 2) return;

        InterpolatedPoints.Clear();
        InterpolatedCurve.Clear();

        var minX = Points.Min(p => p.X);
        var maxX = Points.Max(p => p.X);
        var step = (maxX - minX) / 500.0;

        for (double x = minX; x <= maxX; x += step)
        {
            double y = interpolationCurve.GenerateNewtonInterpolation(x);
            var newPoint = new Point(x, y);
            InterpolatedPoints.Add(newPoint);
        }
        InterpolatedCurve = new PointCollection(InterpolatedPoints);
        OnPropertyChanged(nameof(InterpolatedPoints));
        OnPropertyChanged(nameof(InterpolatedCurve));
    }
}