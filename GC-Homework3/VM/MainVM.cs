using GC_Homework3.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace GC_Homework3.VM;
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
        if (Points.Any() && point.X < Points.Last().X) return;

        Points.Add(point);
        OnPropertyChanged(nameof(Points));
    }
}