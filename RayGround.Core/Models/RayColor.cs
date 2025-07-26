namespace RayGround.Core;

public class RayColor
{
    public const float EPSILON = 0.00001f;

    public readonly float Red;
    public readonly float Green;
    public readonly float Blue;

    RayColor(float red, float green, float blue)
    {
        Red   = red;
        Green = green;
        Blue  = blue;
    }
    
    public static RayColor Create(float red, float green, float blue) =>
        new(red, green, blue);
    
    public override bool Equals(object? obj) =>
        obj is RayColor other
        && MathF.Abs(Red   - other.Red) < EPSILON
        && MathF.Abs(Green - other.Green) < EPSILON
        && MathF.Abs(Blue  - other.Blue) < EPSILON;

    public static bool operator ==(RayColor left, RayColor right) =>
        left.Equals(right);
    
    public static bool operator !=(RayColor left, RayColor right) =>
        !left.Equals(right);
    
    public static RayColor operator +(RayColor left, RayColor right) =>
        new(  left.Red   + right.Red
            , left.Green + right.Green
            , left.Blue  + right.Blue
            );

    public static RayColor operator -(RayColor left, RayColor right) =>
        new(  left.Red   - right.Red
            , left.Green - right.Green
            , left.Blue  - right.Blue
            );

    public static RayColor operator -(RayColor source) =>
        new(-source.Red, -source.Green, -source.Blue);

    public static RayColor operator *(RayColor source, float scaler) =>
        new(  source.Red   * scaler
            , source.Green * scaler
            , source.Blue  * scaler
            );
    
    public static RayColor operator *(RayColor left, RayColor right) =>
        new(  left.Red   * right.Red
            , left.Green * right.Green
            , left.Blue  * right.Blue
            );
    
    public static RayColor operator /(RayColor source, float scaler) =>
        new(  source.Red   / scaler
            , source.Green / scaler
            , source.Blue  / scaler
            );

    public override string ToString() =>
        $"(Red:{Red:F7}, Green:{Green:F7}, Blue:{Blue:F7})";
}
