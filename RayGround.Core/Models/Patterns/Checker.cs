using RayGround.Core.Constants;
using RayGround.Core.Extensions;

namespace RayGround.Core.Models.Patterns;

public class Checker : Pattern
{
    protected Checker(Color primary, Color secondary, Matrix? transform) 
    : base(primary, secondary, transform)
    { }

    public static Checker Create(Color primary, Color secondary, Matrix? transform = null) =>
        new (primary, secondary, transform);

    public static Checker Default() =>
        Create(Color.White, Color.Black, Matrix.Identity);

    public override Color GetColor(Fewple at) => 
        (( MathF.Floor(at.X + Floating.Epsilon) 
         + MathF.Floor(at.Y + Floating.Epsilon)
         + MathF.Floor(at.Z + Floating.Epsilon))
        % 2).NearlyEqual(0) 
            ? Primary 
            : Secondary;
}
