using RayGround.Core.Extensions;

namespace RayGround.Core.Calculators;

public static class IntersectionCalculator
{
    public static Intersection? Hit(Intersection[] intersections)
    {
        var xs = intersections
            .Where(i => i.RayPoint >= 0)
            .OrderBy(i => i.RayPoint)
            .ToArray();
        
        return xs.Length > 0 
            ? xs.First() 
            : null;
    }

    public static Precomputed Precompute(Intersection intersection, Ray with)
    {
        var rayPoint = intersection.RayPoint;
        var collided = (Sphere)intersection.Collided;
        var point    = with.Position(intersection.RayPoint);
        var eye      = -with.Direction;
        var normal   = ((Sphere)intersection.Collided).NormalAt(point);

        var normalEyeDot = normal.Dot(eye); 
        
        return Precomputed.Create(
              rayPoint
            , collided
            , point
            , eye
            , normalEyeDot < 0 ? -normal : normal
            , normalEyeDot < 0 
            );
    }
}
    