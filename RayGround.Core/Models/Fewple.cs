using RayGround.Core.Constants;

namespace RayGround.Core.Models;

public class Fewple
{
    public readonly float X;
    public readonly float Y;
    public readonly float Z;
    public readonly float W;

    protected Fewple(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public static Fewple NewPoint(float x, float y, float z) =>
        new(x, y, z, 1);

    public static Fewple NewVector(float x, float y, float z) =>
        new(x, y, z, 0);

    public static Fewple Create(float x, float y, float z, float w) =>
        new(x, y, z, w);

    public override bool Equals(object? obj) =>
        obj is Fewple other
        && MathF.Abs(X - other.X) < Floating.Epsilon
        && MathF.Abs(Y - other.Y) < Floating.Epsilon
        && MathF.Abs(Z - other.Z) < Floating.Epsilon
        && MathF.Abs(W - other.W) < Floating.Epsilon;

    public static bool operator ==(Fewple left, Fewple right)=>
        left.Equals(right);

    public static bool operator !=(Fewple left, Fewple right) =>
        !(left == right);
    
    public static Fewple operator +(Fewple left, Fewple right) =>
        new(  left.X + right.X
            , left.Y + right.Y
            , left.Z + right.Z
            , left.W + right.W
            );

    public static Fewple operator -(Fewple left, Fewple right) =>
        new(  left.X - right.X
            , left.Y - right.Y
            , left.Z - right.Z
            , MathF.Abs(left.W - right.W)
            );

    public static Fewple operator -(Fewple source) =>
        new(  -source.X
            , -source.Y
            , -source.Z
            , -source.W
            );

    public static Fewple operator *(Fewple source, float scaler) =>
        new(  source.X * scaler
            , source.Y * scaler
            , source.Z * scaler
            , source.W * scaler
            );
    
    public static Fewple operator /(Fewple source, float scaler) =>
        new(  source.X / scaler
            , source.Y / scaler
            , source.Z / scaler
            , source.W / scaler
            );

    public static Fewple operator *(Matrix left, Fewple right) =>
        Create(
              left[0, 0] * right.X + left[0, 1] * right.Y + left[0, 2] * right.Z + left[0, 3] * right.W
            , left[1, 0] * right.X + left[1, 1] * right.Y + left[1, 2] * right.Z + left[1, 3] * right.W
            , left[2, 0] * right.X + left[2, 1] * right.Y + left[2, 2] * right.Z + left[2, 3] * right.W
            , left[3, 0] * right.X + left[3, 1] * right.Y + left[3, 2] * right.Z + left[3, 3] * right.W
            );
    
    public override string ToString() =>
        $"(X:{X}, Y:{Y}, Z:{Z}, W:{W})";
}
