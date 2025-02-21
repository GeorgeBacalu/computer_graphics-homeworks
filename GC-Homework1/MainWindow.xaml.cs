using GC_Homework1.VM;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GC_Homework1;
public partial class MainWindow : Window
{
    private readonly MainVM mainVM;
    private Point lastMousePosition;

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

        var ellipse = new Ellipse { Width = 5, Height = 5, Fill = Brushes.Blue };
        Canvas.SetLeft(ellipse, position.X);
        Canvas.SetTop(ellipse, position.Y);
        DrawingCanvas.Children.Add(ellipse);
    } 

    public void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e) => lastMousePosition = e.GetPosition(DrawingCanvas);

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var scaleFactor = e.Delta > 0 ? 1.1 : 0.9;
        mainVM.ScaleCommand.Execute(new Point(scaleFactor, scaleFactor));
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.RightButton == MouseButtonState.Pressed)
        {
            var currentPosition = e.GetPosition(DrawingCanvas);

            if (lastMousePosition != default)
            {
                var delta = new Point(currentPosition.X - lastMousePosition.X, currentPosition.Y - lastMousePosition.Y);
                lastMousePosition = currentPosition;

                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    mainVM.RotateCommand.Execute(delta.X * 0.01);
                else
                    mainVM.TranslateCommand.Execute(delta);
            }
        }
    }
}