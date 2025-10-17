using RayGround.Core.Models;

namespace RayGround.Core.Extensions;

public static class FewpleExtensions
{
    public static float Magnitude(this Fewple source) =>
        MathF.Sqrt(
              MathF.Pow(source.X, 2)
            + MathF.Pow(source.Y, 2)
            + MathF.Pow(source.Z, 2)
            + MathF.Pow(source.W, 2)
            );

    public static Fewple Normalize(this Fewple source)
    {
        var magnitude = Magnitude(source);
        
        return Fewple.Create(
              source.X / magnitude
            , source.Y / magnitude
            , source.Z / magnitude
            , source.W / magnitude
            );
    }

    public static float Dot(this Fewple left, Fewple right) =>
          left.X * right.X
        + left.Y * right.Y
        + left.Z * right.Z
        + left.W * right.W;

    public static Fewple Cross(this Fewple left, Fewple right) =>
        Fewple.NewVector(
              left.Y * right.Z - left.Z * right.Y
            , left.Z * right.X - left.X * right.Z
            , left.X * right.Y - left.Y * right.X
            );

    public static Fewple Reflect(this Fewple source, Fewple normal) =>
        source - normal * 2 * source.Dot(normal);
    
    public static Matrix ToMatrix(this Fewple source) =>
        new(4, 1)
        { [0, 0] = source.X
        , [1, 0] = source.Y
        , [2, 0] = source.Z
        , [3, 0] = source.W
        };
}