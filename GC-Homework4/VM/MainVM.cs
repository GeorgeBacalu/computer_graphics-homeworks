using GC_Homework4.Commands;
using GC_Homework4.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GC_Homework4.VM;
public class MainVM : BaseVM
{
    private const int PointsCount = 150;

    public enum CurveType { Hermite, Bezier }
    public CurveType CurrentType { get; set; } = CurveType.Hermite;

    public ObservableCollection<Point> Points { get; set; } = new();
    public PathGeometry InterpolatedPath { get; set; } = new();
    public PointCollection ControlPolygonPoints => CurrentType == CurveType.Bezier ? new(Points) : new();

    public ICommand AddPointCommand { get; set; }
    public ICommand GenerateHermiteCurveCommand { get; set; }
    public ICommand GenerateBezierCurveCommand { get; set; }

    public MainVM()
    {
        AddPointCommand = new RelayCommand<Point>(AddPoint);
        GenerateHermiteCurveCommand = new RelayCommand<object>(_ => ApplyCurveType(CurveType.Hermite));
        GenerateBezierCurveCommand = new RelayCommand<object>(_ => ApplyCurveType(CurveType.Bezier));
    }

    private void AddPoint(Point point)
    {
        Points.Add(point);
        OnPropertyChanged(nameof(Points));
        OnPropertyChanged(nameof(ControlPolygonPoints));
    }

    public void MovePoint(int index, Point newPosition)
    {
        if (index < 0 || index >= Points.Count) return;
        if (Points[index] != newPosition)
        {
            Points[index] = newPosition;
            OnPropertyChanged(nameof(Points));
            OnPropertyChanged(nameof(ControlPolygonPoints));
            GenerateCurve();
        }
    }

    private void ApplyCurveType(CurveType type)
    {
        CurrentType = type;
        OnPropertyChanged(nameof(ControlPolygonPoints));
        GenerateCurve();
    }

    private void GenerateCurve()
    {
        if (Points.Count < 2) return;

        var curvePoints = CurrentType switch
        {
            CurveType.Hermite when Points.Count >= 2 && Points.Count % 2 == 0 => InterpolationCurve.GenerateHermiteCurve(Points.ToList(), PointsCount),
            CurveType.Bezier when Points.Count >= 2 => InterpolationCurve.GenerateBezierCurve(Points.ToList(), PointsCount),
            _ => null
        };

        if (curvePoints == null || curvePoints.Count < 2) return;

        var pathFigure = new PathFigure { StartPoint = curvePoints[0], IsClosed = false };
        pathFigure.Segments.Add(new PolyBezierSegment(curvePoints.Skip(1).ToList(), true));

        InterpolatedPath = new PathGeometry(new List<PathFigure> { pathFigure });
        OnPropertyChanged(nameof(InterpolatedPath));
    }
}