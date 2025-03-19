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

    public ObservableCollection<Point> Points { get; set; } = new();
    public PathGeometry HermitePath { get; set; } = new();

    public ICommand AddPointCommand { get; set; }
    public ICommand GenerateHermiteCurveCommand { get; set; }

    public MainVM()
    {
        AddPointCommand = new RelayCommand<Point>(AddPoint);
        GenerateHermiteCurveCommand = new RelayCommand<object>(_ => GenerateHermiteCurve());
    }

    private void AddPoint(Point point)
    {
        Points.Add(point);
        OnPropertyChanged(nameof(Points));
    }

    public void MovePoint(int index, Point newPosition)
    {
        if (index < 0 || index >= Points.Count) return;
        if (Points[index] != newPosition)
        {
            Points[index] = newPosition;
            OnPropertyChanged(nameof(Points));
            GenerateHermiteCurve();
        }
    }
    
    private void GenerateHermiteCurve()
    {
        if (Points.Count <= 2 || Points.Count % 2 == 1) return;

        var pathFigures = new List<PathFigure>();
        for (int i = 0; i < Points.Count - 1; i++)
        {
            var point1 = Points[i];
            var point2 = Points[i + 1];
            var tangent1 = i == 0 ? point2 - point1 : (Points[i + 1] - Points[i - 1]) * 0.5;
            var tangent2 = i == Points.Count - 2 ? point2 - point1 : (Points[i + 2] - Points[i]) * 0.5;

            var curvePoints = HermiteCurve.GenerateCurve(point1, point2, tangent1, tangent2, PointsCount);
            var pathFigure = new PathFigure { StartPoint = curvePoints[0], IsClosed = false };
            var bezierSegment = new PolyBezierSegment(curvePoints.Skip(1).ToList(), true);
            pathFigure.Segments.Add(bezierSegment);
            pathFigures.Add(pathFigure);
        }
        HermitePath = new PathGeometry(pathFigures);
        OnPropertyChanged(nameof(HermitePath));
    }
}