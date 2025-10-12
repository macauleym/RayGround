using Microsoft.VisualBasic;
using RayGround.Core.Constants;
using RayGround.Core.Extensions;

namespace RayGround.Core.Calculators;

public static class IntersectionCalculator
{
    public static Intersection? Hit(Intersection[] intersections)
    {
        var xs = intersections
            .Where(i => i.RayTime >= 0.0)
            .OrderBy(i => i.RayTime)
            .ToArray();
        
        return xs.Length > 0 
            ? xs.First() 
            : null;
    }

    public static Precomputed Precompute(Intersection intersection, Ray with)
    {
        var rayTime  = intersection.RayTime;
        var collided = (Sphere)intersection.Collided;
        var point    = with.Position(rayTime);
        var eye      = -with.Direction;
        var normal   = ((Sphere)intersection.Collided).NormalAt(point);

        var normalEyeDot = normal.Dot(eye);
        var trueNormal   = normalEyeDot < 0 ? -normal : normal;
        var overPoint    = point + trueNormal * FloatingPoint.Epsilon;
        
        return Precomputed.Create(
              rayTime
            , collided
            , point
            , overPoint
            , eye
            , trueNormal
            , normalEyeDot < 0 
            );
    }
}
    