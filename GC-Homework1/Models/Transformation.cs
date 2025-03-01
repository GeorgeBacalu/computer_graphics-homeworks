using MathNet.Numerics.LinearAlgebra;

namespace GC_Homework1.Models;
public class Transformation
{
    public Matrix<double> Matrix { get; private set; }

    public Transformation() => Matrix = Matrix<double>.Build.DenseIdentity(3);

    private Matrix<double> GetRotationMatrix(double cosA, double sinA) => Matrix<double>.Build.DenseOfArray(new double[,]
    {
        { cosA, -sinA, 0 },
        { sinA,  cosA, 0 },
        {  0,     0,   1 }
    });

    private Matrix<double> GetTranslationMatrix(double tx, double ty) => Matrix<double>.Build.DenseOfArray(new double[,]
    {
        { 1, 0, tx },
        { 0, 1, ty },
        { 0, 0, 1  }
    });

    private Matrix<double> GetScalingMatrix(double sx, double sy) => Matrix<double>.Build.DenseOfArray(new double[,]
    {
        { sx, 0,  0 },
        { 0,  sy, 0 },
        { 0,  0,  1 }
    });

    private Matrix<double> GetSymmetryOxMatrix() => Matrix<double>.Build.DenseOfArray(new double[,]
    {
        { 1,  0, 0 },
        { 0, -1, 0 },
        { 0,  0, 1 }
    });

    public void RotateO(double cosA, double sinA) => Matrix = GetRotationMatrix(cosA, sinA) * Matrix;

    public void TranslateO(double tx, double ty) => Matrix = GetTranslationMatrix(tx, ty) * Matrix;

    public void ScaleO(double sx, double sy) => Matrix = GetScalingMatrix(sx, sy) * Matrix;

    public void SymmetryO() => Matrix = GetScalingMatrix(-1, -1) * Matrix;

    public void RotateA(double xa, double ya, double alpha)
    {
        var cosA = Math.Cos(alpha);
        var sinA = Math.Sin(alpha);
        Matrix = GetTranslationMatrix(xa, ya) * GetRotationMatrix(cosA, -sinA) * GetTranslationMatrix(-xa, -ya) * Matrix;
    }

    public void ScaleA(double xa, double ya, double sx, double sy) => Matrix = GetTranslationMatrix(xa, ya) * GetScalingMatrix(sx, sy) * GetTranslationMatrix(-xa, -ya) * Matrix;

    public void SymmetryByLine(double xa, double ya, double vx, double vy)
    {
        var cosA = vx / Math.Sqrt(vx * vx + vy * vy);
        var sinA = vy / Math.Sqrt(vx * vx + vy * vy);
        Matrix = GetTranslationMatrix(xa, ya) * GetRotationMatrix(cosA, -sinA) * GetSymmetryOxMatrix() * GetRotationMatrix(cosA, sinA) * GetTranslationMatrix(-xa, -ya) * Matrix;
    }
}