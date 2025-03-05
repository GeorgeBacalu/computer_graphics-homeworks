using GC_Homework2.VM;
using System.Windows;
using System.Windows.Threading;

namespace GC_Homework2;
public partial class MainWindow : Window
{
    private readonly MainVM mainVM;
    private readonly DispatcherTimer resizeTimer;

    public MainWindow()
    {
        InitializeComponent();
        mainVM = new MainVM();
        DataContext = mainVM;

        resizeTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
        resizeTimer.Tick += (sender, e) =>
        {
            resizeTimer.Stop();
            mainVM.RegenerateCurve();
        };

        DrawingCanvas.SizeChanged += (sender, e) =>
        {
            mainVM.CanvasWidth = DrawingCanvas.ActualWidth;
            mainVM.CanvasHeight = DrawingCanvas.ActualHeight;
            resizeTimer.Stop();
            resizeTimer.Start();
        };
    }
}