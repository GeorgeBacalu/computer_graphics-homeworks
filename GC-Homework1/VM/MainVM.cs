using GC_Homework1.Commands;
using GC_Homework1.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace GC_Homework1.VM;
public class MainVM : BaseVM
{
    private Transformation transformation;

    public ObservableCollection<Point> PolygonPoints { get; } = new();

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
        PolygonPoints.Add(point);
        if (PolygonPoints.Count > 2 && PolygonPoints[0] != PolygonPoints[PolygonPoints.Count - 1])
            PolygonPoints.Add(PolygonPoints[0]);
        OnPropertyChanged(nameof(PolygonPoints));
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
        for (int i = 0; i < PolygonPoints.Count; i++)
        {
            var transformedVector = transformation.Matrix * MathNet.Numerics.LinearAlgebra.Vector<double>.Build.DenseOfArray([PolygonPoints[i].X, PolygonPoints[i].Y, 1]);
            PolygonPoints[i] = new Point(transformedVector[0], transformedVector[1]);
        }
        OnPropertyChanged(nameof(PolygonPoints));
    }

    private Point GetCentroid() => new Point(PolygonPoints.Sum(p => p.X) / PolygonPoints.Count, PolygonPoints.Sum(p => p.Y) / PolygonPoints.Count);
}