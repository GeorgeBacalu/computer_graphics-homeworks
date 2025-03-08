using System.Windows;

namespace GC_Homework3.Models;
public class InterpolationCurve
{
    public List<Point> Points { get; private set; } = new();

    public double GenerateLagrangeInterpolation(double x)
    {
        var result = 0.0;
        for (int i = 0; i < Points.Count; i++)
        {
            var term = Points[i].Y;
            for (int j = 0; j < Points.Count; j++)
                if (j != i)
                    term *= (x - Points[j].X) / (Points[i].X - Points[j].X);

            result += term;
        }
        return result;
    }

    public double GenerateNewtonInterpolation(double x)
    {
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