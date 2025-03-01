using GC_Homework2.VM;
using System.Windows;

namespace GC_Homework2;
public partial class MainWindow : Window
{
    private readonly MainVM mainVM;

    public MainWindow()
    {
        InitializeComponent();
        mainVM = new MainVM();
        DataContext = mainVM;

        DrawingCanvas.SizeChanged += (sender, e) =>
        {
            mainVM.CanvasWidth = DrawingCanvas.ActualWidth;
            mainVM.CanvasHeight = DrawingCanvas.ActualHeight;
        };
    }
}