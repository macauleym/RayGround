namespace RayGround.Core.Models.Patterns;

public class SolidPattern : Pattern
{
    SolidPattern(Color primary) : base(primary, primary) { }

    public static SolidPattern Create(Color toUse) => 
        new(toUse);
    
    public override Color GetColor(Fewple at) => Primary;
}
