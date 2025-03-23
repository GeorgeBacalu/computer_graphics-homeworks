using GC_Homework2.Dtos;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GC_Homework2.Models;
public class Curve
{
    public static List<Point> Points { get; private set; } = new();

    public static List<Point> Generate(Func<double, Point> function, double from, double to, int pointsCount, double width, double height)
    {
        for (int i = 0; i <= pointsCount; i++)
        {
            var step = (to - from) / pointsCount;
            var x = from + i * step;
            Points.Add(function(x));
        }
        return Points;
    }

    public static TransformationResult ApplyTransformations(List<Point> points, double width, double height)
    {
        // 1. Translate points P_i to the first quadrant
        var xMin = points.Min(p => p.X);
        var yMin = points.Min(p => p.Y);
        points = points.Select(p => new Point(p.X - xMin, p.Y - yMin)).ToList();

        // 2. Scale points P'_i to fit the canvas (curve should intersect with top-down or right-left edges)
        var xMax1 = points.Max(p => p.X);
        var yMax1 = points.Max(p => p.Y);
        var sx = width / xMax1;
        var sy = height / yMax1;
        var s = Math.Min(sx, sy);
        points = points.Select(p => new Point(s * p.X, s * p.Y)).ToList();

        // 3. Translate points P''_i to the center of the canvas
        var xMax2 = s * xMax1;
        var yMax2 = s * yMax1;
        var dx = (width - xMax2) / 2;
        var dy = (height - yMax2) / 2;
        points = points.Select(p => new Point(p.X + dx, p.Y + dy)).ToList();

        // 4. Vertical flip
        points = points.Select(p => new Point(p.X, height - p.Y)).ToList();

        // Transform origin and compute axes
        var newOrigin = new Point(-xMin * s + dx, height - (-yMin * s + dy));
        var xStart = new Point(0, newOrigin.Y);
        var xEnd = new Point(width, newOrigin.Y);
        var yStart = new Point(newOrigin.X, 0);
        var yEnd = new Point(newOrigin.X, height);

        var axes = new List<Line>
        {
            new() { X1 = xStart.X, Y1 = xStart.Y, X2 = xEnd.X, Y2 = xEnd.Y, Stroke = Brushes.Black, StrokeThickness = 2 },
            new() { X1 = yStart.X, Y1 = yStart.Y, X2 = yEnd.X, Y2 = yEnd.Y, Stroke = Brushes.Black, StrokeThickness = 2 }
        };
        return new TransformationResult { Points = points, Axes = axes };
    }
}