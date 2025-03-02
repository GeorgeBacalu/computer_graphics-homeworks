using System.Windows;
using System.Windows.Shapes;

namespace GC_Homework2.Dtos;
public class TransformationResult
{
    public List<Point> Points { get; set; } = new();
    public List<Line> Axes { get; set; } = new();
}