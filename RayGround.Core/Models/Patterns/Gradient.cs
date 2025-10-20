namespace RayGround.Core.Models.Patterns;

public class Gradient : Pattern
{
    protected Gradient(Color primary, Color secondary, Matrix? transform) 
    : base(primary, secondary, transform)
    { }

    public static Gradient Create(Color primary, Color secondary, Matrix? transform = null) =>
        new (primary, secondary, transform);

    public static Gradient Default() =>
        Create(Color.White, Color.Black, Matrix.Identity);

    public override Color GetColor(Fewple at)
    {
        var colorDiff = Secondary - Primary;
        var fraction  = at.X - MathF.Floor(at.X);

        return Primary + colorDiff * fraction;
    }
}
