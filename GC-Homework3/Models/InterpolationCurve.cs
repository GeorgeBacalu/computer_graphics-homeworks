using System.Windows;

namespace GC_Homework3.Models;
public class InterpolationCurve
{
    public List<Point> Points { get; private set; } = new();

    public double GenerateLagrangeInterpolation(double x)
    {
        if (!Points.Any()) return 0.0;

        var result = 0.0;
        for (int i = 0; i < Points.Count; i++)
        {
            var term = Points[i].Y;
            for (int j = 0; j < Points.Count; j++)
                if (j != i)
                {
                    var denominator = (Points[i].X - Points[j].X);
                    if (denominator == 0) return 0;
                    term *= (x - Points[j].X) / denominator;
                }
            result += term;
        }
        return result;
    }

    public double GenerateNewtonInterpolation(double x)
    {
        if (!Points.Any()) return 0.0;

        var dividedDifferences = new double[Points.Count];
        for (int i = 0; i < Points.Count; i++)
            dividedDifferences[i] = Points[i].Y;

        for (int j = 1; j < Points.Count; j++)
            for (int i = Points.Count - 1; i >= j; i--)
                dividedDifferences[i] = (dividedDifferences[i] - dividedDifferences[i - 1]) / (Points[i].X - Points[i - j].X);

        var result = dividedDifferences[Points.Count - 1];
        for (int i = Points.Count - 2; i >= 0; i--)
            result = result * (x - Points[i].X) + dividedDifferences[i];
        return result;
    }
}