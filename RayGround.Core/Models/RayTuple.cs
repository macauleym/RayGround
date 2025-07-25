namespace RayGround.Core;

public class RayTuple
{
    public const float EPSILON = 0.00001f;
    
    public readonly float X;
    public readonly float Y;
    public readonly float Z;
    public readonly float W;

    RayTuple(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public static RayTuple NewPoint(float x, float y, float z) =>
        new(x, y, z, 1);

    public static RayTuple NewVector(float x, float y, float z) =>
        new(x, y, z, 0);

    public static RayTuple Create(float x, float y, float z, float w) =>
        new(x, y, z, w);

    public override bool Equals(object? obj) =>
        obj is RayTuple other
        && MathF.Abs(X - other.X) < EPSILON
        && MathF.Abs(Y - other.Y) < EPSILON
        && MathF.Abs(Z - other.Z) < EPSILON
        && MathF.Abs(W - other.W) < EPSILON;

    public static bool operator ==(RayTuple left, RayTuple right)=>
        left.Equals(right);

    public static bool operator !=(RayTuple left, RayTuple right) =>
        !(left == right);
    
    public static RayTuple operator +(RayTuple left, RayTuple right) =>
        new(  left.X + right.X
            , left.Y + right.Y
            , left.Z + right.Z
            , left.W + right.W
            );

    public static RayTuple operator -(RayTuple left, RayTuple right) =>
        new(  left.X - right.X
            , left.Y - right.Y
            , left.Z - right.Z
            , MathF.Abs(left.W - right.W)
            );

    public static RayTuple operator -(RayTuple source) =>
        new(  -source.X
            , -source.Y
            , -source.Z
            , -source.W
            );

    public static RayTuple operator *(RayTuple source, float scaler) =>
        new(  source.X * scaler
            , source.Y * scaler
            , source.Z * scaler
            , source.W * scaler
            );
    
    public static RayTuple operator /(RayTuple source, float scaler) =>
        new(  source.X / scaler
            , source.Y / scaler
            , source.Z / scaler
            , source.W / scaler
            );

    public override string ToString() =>
        $"(X:{X}, Y:{Y}, Z:{Z}, W:{W})";
}
