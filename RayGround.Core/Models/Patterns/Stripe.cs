namespace RayGround.Core.Models.Patterns;

public class Stripe : Pattern
{
    protected Stripe(Color primary, Color secondary) : base(primary, secondary) { }

    public static Stripe Create(Color primary, Color secondary) =>
        new(primary, secondary);

    public static Stripe Default() => 
        Create(Color.White, Color.Black);

    public override Color GetColor(Fewple at) =>
        MathF.Floor(at.X) % 2 == 0
            ? Primary
            : Secondary;
}
