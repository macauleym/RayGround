using RayGround.Core.Extensions;

namespace RayGround.Core.Models.Patterns;

public class Rings : Pattern
{
    protected Rings(Color primary, Color secondary, Matrix? transform) 
    : base(primary, secondary, transform)
    { }

    public static Rings Create(Color primary, Color secondary, Matrix? transform = null) =>
        new(primary, secondary, transform);

    public static Rings Default() =>
        Create(Color.White, Color.Black, Matrix.Identity);

    public override Color GetColor(Fewple at)
    {
        var inv = at;
        return (
            MathF.Floor(
                MathF.Sqrt(
                    inv.X * inv.X + inv.Z * inv.Z)) 
            % 2).NearlyEqual(0) 
            ? Primary 
            : Secondary;
    }
}
