using RayGround.Core.Models;

namespace RayGround.Core;

public class Precomputed
{
    public readonly float RayPoint;
    public readonly Entity Collided;
    public readonly Fewple Point;
    public readonly Fewple OverPoint;
    public readonly Fewple EyeVector;
    public readonly Fewple NormalVector;
    public readonly bool IsInside;
    
    Precomputed(
      float rayPoint
    , Entity collided
    , Fewple point
    , Fewple overPoint
    , Fewple eyeVector
    , Fewple normalVector
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
    , Entity collided
    , Fewple point
    , Fewple overPoint
    , Fewple eyeVector
    , Fewple normalVector
    , bool isInside
    ) => new(rayPoint, collided, point, overPoint, eyeVector, normalVector, isInside);
}
