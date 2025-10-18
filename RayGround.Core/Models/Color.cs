namespace RayGround.Core;

public class Color
{
    public const float EPSILON = 0.00001f;

    public static readonly Color Black = new (0, 0, 0);

    public readonly float Red;
    public readonly float Green;
    public readonly float Blue;

    Color(float red, float green, float blue)
    {
        Red   = red;
        Green = green;
        Blue  = blue;
    }
    
    public static Color Create(float red, float green, float blue) =>
        new(red, green, blue);

    public static Color Uniform(float rgb) =>
        new(rgb, rgb, rgb);
    
    public override bool Equals(object? obj) =>
        obj is Color other
        && MathF.Abs(Red   - other.Red) < EPSILON
        && MathF.Abs(Green - other.Green) < EPSILON
        && MathF.Abs(Blue  - other.Blue) < EPSILON;

    public static bool operator ==(Color left, Color right) =>
        left.Equals(right);
    
    public static bool operator !=(Color left, Color right) =>
        !left.Equals(right);
    
    public static Color operator +(Color left, Color right) =>
        new(  left.Red   + right.Red
            , left.Green + right.Green
            , left.Blue  + right.Blue
            );

    public static Color operator -(Color left, Color right) =>
        new(  left.Red   - right.Red
            , left.Green - right.Green
            , left.Blue  - right.Blue
            );

    public static Color operator -(Color source) =>
        new(-source.Red, -source.Green, -source.Blue);

    public static Color operator *(Color source, float scaler) =>
        new(  source.Red   * scaler
            , source.Green * scaler
            , source.Blue  * scaler
            );
    
    public static Color operator *(Color left, Color right) =>
        new(  left.Red   * right.Red
            , left.Green * right.Green
            , left.Blue  * right.Blue
            );
    
    public static Color operator /(Color source, float scaler) =>
        new(  source.Red   / scaler
            , source.Green / scaler
            , source.Blue  / scaler
            );

    public override string ToString() =>
        $"(Red:{Red:F7}, Green:{Green:F7}, Blue:{Blue:F7})";
}
