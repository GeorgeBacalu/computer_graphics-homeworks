using System.Windows;

namespace GC_Homework4.Models;
public class InterpolationCurve
{
    public static List<Point> GenerateHermiteCurve(List<Point> points, int pointsCount)
    {
        var curvePoints = new List<Point>();
        for (int i = 0; i < points.Count - 1; i++)
        {
            var point1 = points[i];
            var point2 = points[i + 1];
            var tangent1 = i == 0 ? point2 - point1 : (points[i + 1] - points[i - 1]) * 0.5;
            var tangent2 = i == points.Count - 2 ? point2 - point1 : (points[i + 2] - points[i]) * 0.5;

            for (int j = 0; j <= pointsCount; j++)
            {
                var u = j / (double)pointsCount;
                var h1 = 2 * Math.Pow(u, 3) - 3 * Math.Pow(u, 2) + 1;
                var h2 = -2 * Math.Pow(u, 3) + 3 * Math.Pow(u, 2);
                var h3 = Math.Pow(u, 3) - 2 * Math.Pow(u, 2) + u;
                var h4 = Math.Pow(u, 3) - Math.Pow(u, 2);
                var x = h1 * point1.X + h2 * point2.X + h3 * tangent1.X + h4 * tangent2.X;
                var y = h1 * point1.Y + h2 * point2.Y + h3 * tangent1.Y + h4 * tangent2.Y;
                curvePoints.Add(new Point(x, y));
            }
        }
        return curvePoints;
    }

    public static List<Point> GenerateBezierCurve(List<Point> controlPoints, int pointsCount)
    {
        int n = controlPoints.Count - 1;
        var curvePoints = new List<Point>();
        for (int i = 0; i <= pointsCount; i++)
        {
            double u = i / (double)pointsCount;
            double x = 0.0, y = 0.0;

            for (int j = 0; j <= n; j++)
            {
                var binomial = Factorial(n) / (Factorial(j) * Factorial(n - j));
                var b = binomial * Math.Pow(u, j) * Math.Pow(1 - u, n - j);
                x += b * controlPoints[j].X;
                y += b * controlPoints[j].Y;
            }
            curvePoints.Add(new Point(x, y));
        }
        return curvePoints;
    }

    private static double Factorial(int n)
    {
        double result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;
        return result;
    }
}