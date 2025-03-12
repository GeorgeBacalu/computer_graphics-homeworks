using System.Windows;

namespace GC_Homework3.Models;
public class InterpolationCurve
{
    public static List<Point> Points { get; private set; } = new();

    public static double GenerateLagrangeInterpolation(double x)
    {
        if (!Points.Any()) return 0.0;

        var finalY = 0.0;
        for (int i = 0; i < Points.Count; i++)
        {
            var y = Points[i].Y;
            for (int j = 0; j < Points.Count; j++)
                if (j != i)
                    y *= (x - Points[j].X) / (Points[i].X - Points[j].X);
            finalY += y;
        }
        return finalY;
    }

    public static double GenerateNewtonInterpolation(double x)
    {
        if (Points.Count == 0) return 0.0;

        int n = Points.Count;
        var matrix = new double[n, n];

        // 1. Initialize the matrix with y_i values on the main diagonal
        for (int i = 0; i < n; i++)
            matrix[i, i] = Points[i].Y;

        // 2. Compute the divided difference coefficients (construct the upper triangular matrix)
        for (int j = 1; j < n; j++)
            for (int i = 0; i < n - j; i++)
                matrix[i, i + j] = (matrix[i + 1, i + j] - matrix[i, i + j - 1]) / (Points[i + j].X - Points[i].X);

        // 3. Evaluate the polynomial at point x
        double result = matrix[0, n - 1];
        for (int i = n - 2; i >= 0; i--)
            result = result * (x - Points[i].X) + matrix[0, i];

        return result;
    }
}