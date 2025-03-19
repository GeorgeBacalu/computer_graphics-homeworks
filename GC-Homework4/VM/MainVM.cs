using GC_Homework4.Commands;
using GC_Homework4.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace GC_Homework4.VM;
public class MainVM : BaseVM
{
    public ObservableCollection<Point> Points { get; set; } = new();

    public ICommand AddPointCommand { get; set; }

    public MainVM()
    {
        AddPointCommand = new RelayCommand<Point>(AddPoint);
    }

    private void AddPoint(Point point)
    {
        Points.Add(point);
        HermiteCurve.Points.Add(point);

        OnPropertyChanged(nameof(Points));
    }
}