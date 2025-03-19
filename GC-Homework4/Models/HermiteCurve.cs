using System.Windows;

namespace GC_Homework4.Models;
public class HermiteCurve
{
    public static List<Point> Points { get; private set; } = new();

    public static List<Point> GenerateCurve(Point point1, Point point2, Vector tangent1, Vector tangent2, int pointsCount)
    {
        for (int i = 0; i <= pointsCount; i++)
        {
            var u = i / (double)pointsCount;
            var h1 = 2 * Math.Pow(u, 3) - 3 * Math.Pow(u, 2) + 1;
            var h2 = -2 * Math.Pow(u, 3) + 3 * Math.Pow(u, 2);
            var h3 = Math.Pow(u, 3) - 2 * Math.Pow(u, 2) + u;
            var h4 = Math.Pow(u, 3) - Math.Pow(u, 2);
            var x = h1 * point1.X + h2 * point2.X + h3 * tangent1.X + h4 * tangent2.X;
            var y = h1 * point1.Y + h2 * point2.Y + h3 * tangent1.Y + h4 * tangent2.Y;
            Points.Add(new Point(x, y));
        }
        return Points;
    }
}