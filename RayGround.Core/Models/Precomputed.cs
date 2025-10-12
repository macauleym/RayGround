namespace RayGround.Core;

public class Precomputed
{
    public readonly float RayPoint;
    public readonly Sphere Collided;
    public readonly RayTuple Point;
    public readonly RayTuple OverPoint;
    public readonly RayTuple EyeVector;
    public readonly RayTuple NormalVector;
    public readonly bool IsInside;
    
    Precomputed(
      float rayPoint
    , Sphere collided
    , RayTuple point
    , RayTuple overPoint
    , RayTuple eyeVector
    , RayTuple normalVector
    , bool isInside
    ) {
        RayPoint     = rayPoint;
        Collided     = collided;
        Point        = point;
        OverPoint    = overPoint;
        EyeVector    = eyeVector;
        NormalVector = normalVector;
        IsInside     = isInside;
    }
    
    public static Precomputed Create(
      float rayPoint
    , Sphere collided
    , RayTuple point
    , RayTuple overPoint
    , RayTuple eyeVector
    , RayTuple normalVector
    , bool isInside
    ) => new(rayPoint, collided, point, overPoint, eyeVector, normalVector, isInside);
}
