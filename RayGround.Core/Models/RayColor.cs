namespace RayGround.Core;

public struct RayColor(float red, float green, float blue)
{
    public const float EPSILON = 0.00001f;

    public readonly float Red   = red;
    public readonly float Green = green;
    public readonly float Blue  = blue;

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
        $"(Red:{Red}, Green:{Green}, Blue:{Blue})";
}
