using GC_Homework1.Commands;
using GC_Homework1.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GC_Homework1.VM;
public class MainVM : BaseVM
{
    private Transformation transformation = new();
    private const double pointRadius = 2.5, rotationSensitivity = 0.005, translationSensitivity = 0.005;

    public ObservableCollection<Point> Points { get; } = new();
    public ObservableCollection<Line> Lines { get; } = new();

    public ICommand AddPointCommand { get; set; }
    public ICommand RotateCommand { get; set; }
    public ICommand TranslateCommand { get; set; }
    public ICommand ScaleCommand { get; set; }

    public MainVM()
    {
        AddPointCommand = new RelayCommand<Point>(AddPoint);
        RotateCommand = new RelayCommand<double>(Rotate);
        TranslateCommand = new RelayCommand<Point>(Translate);
        ScaleCommand = new RelayCommand<Point>(Scale);
    }

    private void AddPoint(Point point)
    {
        Points.Add(point);

        int count = Points.Count;
        if (count == 2)
            Lines.Add(GetLine(Points[0], Points[1]));
        if (count == 3)
        {
            Lines.Add(GetLine(Points[1], Points[2]));
            Lines.Add(GetLine(Points[0], Points[2]));
        }
        else if (count > 3)
        {
            Lines.Add(GetLine(Points[count - 2], Points[count - 1]));
            Lines.Add(GetLine(Points[0], Points[count - 1]));

            var lineToRemove = Lines.FirstOrDefault(line => line.X1 == Points[0].X + pointRadius && line.Y1 == Points[0].Y + pointRadius && line.X2 == Points[count - 2].X + pointRadius && line.Y2 == Points[count - 2].Y + pointRadius);
            if (lineToRemove != null)
                Lines.Remove(lineToRemove);
        }
        OnPropertyChanged(nameof(Points));
        OnPropertyChanged(nameof(Lines));
    }

    private void Rotate(double alpha)
    {
        var centroid = GetCentroid();
        transformation.RotateA(centroid.X, centroid.Y, alpha * rotationSensitivity);
        ApplyTransformation();
    }

    private void Translate(Point delta)
    {
        transformation.TranslateO(delta.X * translationSensitivity, delta.Y * translationSensitivity);
        ApplyTransformation();
    }

    private void Scale(Point scale)
    {
        var centroid = GetCentroid();
        transformation.ScaleA(centroid.X, centroid.Y, scale.X, scale.Y);
        ApplyTransformation();
    }

    private void ApplyTransformation()
    {
        var newPoints = new List<Point>();
        foreach (var point in Points)
        {
            var transformed = transformation.Matrix * MathNet.Numerics.LinearAlgebra.Vector<double>.Build.DenseOfArray([point.X, point.Y, 1]);
            newPoints.Add(new Point(transformed[0], transformed[1]));
        }
        Points.Clear();
        newPoints.ForEach(Points.Add);

        Lines.Clear();
        for (int i = 1; i < Points.Count; i++)
            Lines.Add(GetLine(Points[i - 1], Points[i]));
        if (Points.Count > 2)
            Lines.Add(GetLine(Points[0], Points[Points.Count - 1]));

        OnPropertyChanged(nameof(Points));
        OnPropertyChanged(nameof(Lines));
    }

    private Line GetLine(Point point1, Point point2) => new() { X1 = point1.X + pointRadius, Y1 = point1.Y + pointRadius, X2 = point2.X + pointRadius, Y2 = point2.Y + pointRadius, Stroke = Brushes.Blue, StrokeThickness = 2 };

    private Point GetCentroid() => new(Points.Average(p => p.X), Points.Average(p => p.Y));
}