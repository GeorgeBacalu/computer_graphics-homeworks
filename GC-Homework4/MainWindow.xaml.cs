using GC_Homework4.VM;
using System.Windows;
using System.Windows.Input;

namespace GC_Homework4;
public partial class MainWindow : Window
{
    private readonly MainVM mainVM;

    public MainWindow()
    {
        InitializeComponent();
        mainVM = new MainVM();
        DataContext = mainVM;
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var position = e.GetPosition(DrawingCanvas);
        mainVM.AddPointCommand.Execute(position);
    }

    private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        var position = e.GetPosition(DrawingCanvas);
        mainVM.GenerateHermiteCurveCommand.Execute(position);
    }
}