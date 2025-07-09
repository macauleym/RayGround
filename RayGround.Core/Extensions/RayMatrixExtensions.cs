namespace RayGround.Core.Extensions;

public static class RayMatrixExtensions
{
    public static T Traverse<T>(this RayMatrix source, T seed, Func<T, int, int, T> traverser)
    {
        var acc = seed;
        for (var r = 0; r < source.Rows; r++)
        for (var c = 0; c < source.Columns; c++)
            acc = traverser(acc, r, c);

        return acc;
    }

    public static RayTuple ToTuple(this RayMatrix source) =>
        new(
          source[0, 0]
        , source[1, 0]
        , source[2, 0]
        , source[3, 0]
        );
}
