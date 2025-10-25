using RayGround.Core.Extensions;

namespace RayGround.Core.Models.Patterns;

public class Rings : Pattern
{
    protected Rings(Color primary, Color secondary) : base(primary, secondary) { }

    public static Rings Create(Color primary, Color secondary) =>
        new(primary, secondary);

    public static Rings Default() =>
        Create(Color.White, Color.Black);

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
