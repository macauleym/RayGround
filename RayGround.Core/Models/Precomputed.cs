namespace RayGround.Core;

public class Precomputed
{
    public readonly float RayPoint;
    public readonly Sphere Collided;
    public readonly RayTuple Point;
    public readonly RayTuple EyeVector;
    public readonly RayTuple NormalVector;
    public readonly bool IsInside;
    
    Precomputed(
      float rayPoint
    , Sphere collided
    , RayTuple point
    , RayTuple eyeVector
    , RayTuple normalVector
    , bool isInside
    ) {
        RayPoint     = rayPoint;
        Collided     = collided;
        Point        = point;
        EyeVector    = eyeVector;
        NormalVector = normalVector;
        IsInside     = isInside;
    }
    
    public static Precomputed Create(
      float rayPoint
    , Sphere collided
    , RayTuple point
    , RayTuple eyeVector
    , RayTuple normalVector
    , bool isInside
    ) => new(rayPoint, collided, point, eyeVector, normalVector, isInside);
}
