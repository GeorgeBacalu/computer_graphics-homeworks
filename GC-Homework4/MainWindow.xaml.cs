using GC_Homework4.VM;
using System.Windows;
using System.Windows.Input;

namespace GC_Homework4;
public partial class MainWindow : Window
{
    private readonly MainVM mainVM;
    private int? selectedPointIndex = null;

    public MainWindow()
    {
        InitializeComponent();
        mainVM = new MainVM();
        DataContext = mainVM;
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var position = e.GetPosition(DrawingCanvas);
        selectedPointIndex = mainVM.Points.Select((p, i) => new { Point = p, Index = i }).OrderBy(p => GetDistance(p.Point, position)).FirstOrDefault()?.Index;
        if (selectedPointIndex.HasValue && GetDistance(mainVM.Points[selectedPointIndex.Value], position) < 100) DrawingCanvas.CaptureMouse();
        else mainVM.AddPointCommand.Execute(position);
    }

    private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        var position = e.GetPosition(DrawingCanvas);
        if (Keyboard.IsKeyDown(Key.LeftCtrl))
            mainVM.GenerateBezierCurveCommand.Execute(position);
        else
            mainVM.GenerateHermiteCurveCommand.Execute(position);
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (selectedPointIndex.HasValue && e.LeftButton == MouseButtonState.Pressed)
        {
            var position = e.GetPosition(DrawingCanvas);
            mainVM.MovePoint(selectedPointIndex.Value, position);
        }
    }

    private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (selectedPointIndex.HasValue)
        {
            selectedPointIndex = null;
            DrawingCanvas.ReleaseMouseCapture();
        }
    }

    private double GetDistance(Point p1, Point p2) => Math.Pow((p1.X - p2.X), 2) + Math.Pow((p1.Y - p2.Y), 2);
}