using RayGround.Core.Models.Entities;

namespace RayGround.Core.Models;

public class Precomputed
{
    public readonly float RayPoint;
    public readonly Entity Collided;
    public readonly Fewple Point;
    public readonly Fewple OverPoint;
    public readonly Fewple UnderPoint;
    public readonly Fewple EyeVector;
    public readonly Fewple NormalVector;
    public readonly Fewple ReflectVector;
    public readonly float RefractionFrom;
    public readonly float RefractionTo;
    public readonly bool IsInside;
    
    Precomputed(
      float rayPoint
    , Entity collided
    , Fewple point
    , Fewple overPoint
    , Fewple underPoint
    , Fewple eyeVector
    , Fewple normalVector
    , Fewple reflectVector
    , float refractionFrom
    , float refractionTo
    , bool isInside
    ) {
        RayPoint       = rayPoint;
        Collided       = collided;
        Point          = point;
        OverPoint      = overPoint;
        UnderPoint     = underPoint;
        EyeVector      = eyeVector;
        NormalVector   = normalVector;
        ReflectVector  = reflectVector;
        RefractionFrom = refractionFrom;
        RefractionTo   = refractionTo;
        IsInside       = isInside;
    }
    
    public static Precomputed Create(
      float rayPoint
    , Entity collided
    , Fewple point
    , Fewple overPoint
    , Fewple underPoint
    , Fewple eyeVector
    , Fewple normalVector
    , Fewple reflectVector
    , float refractionFrom
    , float refractionTo
    , bool isInside
    ) => new(
          rayPoint
        , collided
        , point
        , overPoint
        , underPoint
        , eyeVector
        , normalVector
        , reflectVector
        , refractionFrom
        , refractionTo
        , isInside
        );
}
