using RayGround.Core.Calculators;
using RayGround.Core.Operations;

namespace RayGround.Core.Extensions;

public static class RayMatrixExtensions
{
    public static T TraverseI<T>(this RayMatrix source, T seed, Func<T, (int, int), float, T> traverserI)
    {
        var acc = seed;
        for (var r = 0; r < source.Rows; r++)
        for (var c = 0; c < source.Columns; c++)
            acc = traverserI(acc, (r, c), source[r, c]);

        return acc;
    }
    
    public static T Traverse<T>(this RayMatrix source, T seed, Func<T, float, T> traverser)
    {
        var acc = seed;
        for (var r = 0; r < source.Rows; r++)
        for (var c = 0; c < source.Columns; c++)
            acc = traverser(acc, source[r, c]);

        return acc;
    }

    public static bool Any(this RayMatrix source, Predicate<float> check) =>
        Traverse(source, false, (acc, current) => acc || check(current));

    public static bool All(this RayMatrix source, Predicate<float> check) =>
        Traverse(source, true, (acc, current) => acc && check(current));

    public static RayMatrix Transpose(this RayMatrix of) =>
        MatrixCalculator.Transpose(of);

    public static float Determinant(this RayMatrix of) =>
        MatrixCalculator.Determinant(of);

    public static RayMatrix Submatrix(this RayMatrix of, int targetRow, int targetColumn) =>
        MatrixCalculator.Submatrix(of, targetRow, targetColumn);

    public static float Minor(this RayMatrix of, int targetRow, int targetColumn) =>
        of.Submatrix(targetRow, targetColumn)
          .Determinant();

    public static float Cofactor(this RayMatrix of, int targetRow, int targetColumn) =>
        MatrixCalculator.Cofactor(of, targetRow, targetColumn);

    public static bool IsInvertible(this RayMatrix of) =>
        MatrixCalculator.IsInvertible(of);

    public static RayMatrix Inverse(this RayMatrix of) =>
        MatrixCalculator.Inverse(of);
    
    public static RayTuple ToTuple(this RayMatrix source) =>
        new(
          source[0, 0]
        , source[1, 0]
        , source[2, 0]
        , source[3, 0]
        );
}
