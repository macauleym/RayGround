namespace RayGround.Core;

public readonly struct RayTuple(float x, float y, float z, float w)
{
    public const float EPSILON = 0.00001f;
    
    public readonly float X = x;
    public readonly float Y = y;
    public readonly float Z = z;
    public readonly float W = w;

    public static RayTuple NewPoint(float x, float y, float z) =>
        new(x, y, z, 1);

    public static RayTuple NewVector(float x, float y, float z) =>
        new(x, y, z, 0);

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
