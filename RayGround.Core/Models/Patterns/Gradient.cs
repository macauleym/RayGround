namespace RayGround.Core.Models.Patterns;

public class Gradient : Pattern
{
    protected Gradient(Color primary, Color secondary) : base(primary, secondary) { }

    public static Gradient Create(Color primary, Color secondary) =>
        new (primary, secondary);

    public static Gradient Default() =>
        Create(Color.White, Color.Black);

    public override Color GetColor(Fewple at)
    {
        var colorDiff = Secondary - Primary;
        var fraction  = at.X - MathF.Floor(at.X);

        return Primary + colorDiff * fraction;
    }
}
