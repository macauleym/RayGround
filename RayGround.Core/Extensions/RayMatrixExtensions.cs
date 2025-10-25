using RayGround.Core.Calculators;
using RayGround.Core.Models;

namespace RayGround.Core.Extensions;

public static class RayMatrixExtensions
{
    public static T TraverseI<T>(this Matrix source, T seed, Func<T, (int, int), float, T> traverserI)
    {
        var acc = seed;
        for (var r = 0; r < source.Rows; r++)
        for (var c = 0; c < source.Columns; c++)
            acc = traverserI(acc, (r, c), source[r, c]);

        return acc;
    }
    
    public static T Traverse<T>(this Matrix source, T seed, Func<T, float, T> traverser)
    {
        var acc = seed;
        for (var r = 0; r < source.Rows; r++)
        for (var c = 0; c < source.Columns; c++)
            acc = traverser(acc, source[r, c]);

        return acc;
    }

    public static bool Any(this Matrix source, Predicate<float> check) =>
        Traverse(source, false, (acc, current) => acc || check(current));

    public static bool All(this Matrix source, Predicate<float> check) =>
        Traverse(source, true, (acc, current) => acc && check(current));

    public static Matrix Transpose(this Matrix of) =>
        MatrixCalculator.Transpose(of);

    public static float Determinant(this Matrix of) =>
        MatrixCalculator.Determinant(of);

    public static Matrix Submatrix(this Matrix of, int targetRow, int targetColumn) =>
        MatrixCalculator.Submatrix(of, targetRow, targetColumn);

    public static float Minor(this Matrix of, int targetRow, int targetColumn) =>
        of.Submatrix(targetRow, targetColumn)
          .Determinant();

    public static float Cofactor(this Matrix of, int targetRow, int targetColumn) =>
        MatrixCalculator.Cofactor(of, targetRow, targetColumn);

    public static bool IsInvertible(this Matrix of) =>
        MatrixCalculator.IsInvertible(of);

    public static Matrix Inverse(this Matrix of) =>
        MatrixCalculator.Inverse(of);
    
    public static Fewple ToTuple(this Matrix source) =>
        Fewple.Create(
          source[0, 0]
        , source[1, 0]
        , source[2, 0]
        , source[3, 0]
        );
}
