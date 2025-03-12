using MathNet.Numerics.LinearAlgebra;

namespace GC_Homework1.Models;
public class Transformation
{
    public static Matrix<double> Matrix { get; private set; } = Matrix<double>.Build.DenseIdentity(3);

    public static void RotateO(double cosA, double sinA) => Matrix = GetRotationMatrix(cosA, sinA) * Matrix;

    public static void TranslateO(double tx, double ty) => Matrix = GetTranslationMatrix(tx, ty) * Matrix;

    public static void ScaleO(double sx, double sy) => Matrix = GetScalingMatrix(sx, sy) * Matrix;

    public static void SymmetryO() => Matrix = GetScalingMatrix(-1, -1) * Matrix;

    public static void RotateA(double xa, double ya, double alpha)
    {
        var cosA = Math.Cos(alpha);
        var sinA = Math.Sin(alpha);
        Matrix = GetTranslationMatrix(xa, ya) * GetRotationMatrix(cosA, -sinA) * GetTranslationMatrix(-xa, -ya) * Matrix;
    }

    public static void ScaleA(double xa, double ya, double sx, double sy) => Matrix = GetTranslationMatrix(xa, ya) * GetScalingMatrix(sx, sy) * GetTranslationMatrix(-xa, -ya) * Matrix;

    public static void SymmetryByLine(double xa, double ya, double vx, double vy)
    {
        var cosA = vx / Math.Sqrt(vx * vx + vy * vy);
        var sinA = vy / Math.Sqrt(vx * vx + vy * vy);
        Matrix = GetTranslationMatrix(xa, ya) * GetRotationMatrix(cosA, -sinA) * GetSymmetryOxMatrix() * GetRotationMatrix(cosA, sinA) * GetTranslationMatrix(-xa, -ya) * Matrix;
    }

    private static Matrix<double> GetRotationMatrix(double cosA, double sinA) => Matrix<double>.Build.DenseOfArray(new double[,]
    {
        { cosA, -sinA, 0 },
        { sinA,  cosA, 0 },
        {  0,     0,   1 }
    });

    private static Matrix<double> GetTranslationMatrix(double tx, double ty) => Matrix<double>.Build.DenseOfArray(new double[,]
    {
        { 1, 0, tx },
        { 0, 1, ty },
        { 0, 0, 1  }
    });

    private static Matrix<double> GetScalingMatrix(double sx, double sy) => Matrix<double>.Build.DenseOfArray(new double[,]
    {
        { sx, 0,  0 },
        { 0,  sy, 0 },
        { 0,  0,  1 }
    });

    private static Matrix<double> GetSymmetryOxMatrix() => Matrix<double>.Build.DenseOfArray(new double[,]
    {
        { 1,  0, 0 },
        { 0, -1, 0 },
        { 0,  0, 1 }
    });
}