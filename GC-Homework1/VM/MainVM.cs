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
    private Transformation transformation;
    private const double pointRadius = 2.5;

    public ObservableCollection<Point> Points { get; } = new();
    public ObservableCollection<Line> Lines { get; } = new();

    public ICommand AddPointCommand { get; set; }
    public ICommand RotateCommand { get; set; }
    public ICommand TranslateCommand { get; set; }
    public ICommand ScaleCommand { get; set; }

    public MainVM()
    {
        transformation = new Transformation();
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
        transformation.RotateA(centroid.X, centroid.Y, alpha);
        ApplyTransformation();
    }

    private void Translate(Point translateFactors)
    {
        transformation.TranslateO(translateFactors.X, translateFactors.Y);
        ApplyTransformation();
    }

    private void Scale(Point scaleFactors)
    {
        var centroid = GetCentroid();
        transformation.ScaleA(centroid.X, centroid.Y, scaleFactors.X, scaleFactors.Y);
        ApplyTransformation();
    }

    private void ApplyTransformation()
    {
        for (int i = 0; i < Points.Count; i++)
        {
            var transformedVector = transformation.Matrix * MathNet.Numerics.LinearAlgebra.Vector<double>.Build.DenseOfArray([Points[i].X, Points[i].Y, 1]);
            Points[i] = new Point(transformedVector[0], transformedVector[1]);
        }
        OnPropertyChanged(nameof(Points));
    }

    private Line GetLine(Point point1, Point point2) => new Line { X1 = point1.X + pointRadius, Y1 = point1.Y + pointRadius, X2 = point2.X + pointRadius, Y2 = point2.Y + pointRadius, Stroke = Brushes.Blue, StrokeThickness = 2 };

    private Point GetCentroid() => new Point(Points.Sum(p => p.X) / Points.Count, Points.Sum(p => p.Y) / Points.Count);
}