namespace RayGround.Core.Extensions;

public static class RayTupleExtensions
{
    public static float Magnitude(this RayTuple source) =>
        MathF.Sqrt(
              MathF.Pow(source.X, 2)
            + MathF.Pow(source.Y, 2)
            + MathF.Pow(source.Z, 2)
            + MathF.Pow(source.W, 2)
            );

    public static RayTuple Normalize(this RayTuple source)
    {
        var magnitude = Magnitude(source);
        
        return RayTuple.Create(
              source.X / magnitude
            , source.Y / magnitude
            , source.Z / magnitude
            , source.W / magnitude
            );
    }

    public static float Dot(this RayTuple left, RayTuple right) =>
          left.X * right.X
        + left.Y * right.Y
        + left.Z * right.Z
        + left.W * right.W;

    public static RayTuple Cross(this RayTuple left, RayTuple right) =>
        RayTuple.NewVector(
              left.Y * right.Z - left.Z * right.Y
            , left.Z * right.X - left.X * right.Z
            , left.X * right.Y - left.Y * right.X
            );

    public static RayTuple Reflect(this RayTuple source, RayTuple normal) =>
        source - normal * 2 * source.Dot(normal);
    
    public static RayMatrix ToMatrix(this RayTuple source) =>
        new(4, 1)
        { [0, 0] = source.X
        , [1, 0] = source.Y
        , [2, 0] = source.Z
        , [3, 0] = source.W
        };
}