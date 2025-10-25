using RayGround.Core.Constants;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Models.Entities;

namespace RayGround.Core.Calculators;

public static class IntersectionCalculator
{
    public static Intersection? Hit(Intersection[] intersections)
    {
        var xs = intersections
            .Where(i => i.RayTime >= 0.0f)
            .OrderBy(i => i.RayTime)
            .ToArray();
        
        return xs.Length > 0 
            ? xs.First() 
            : null;
    }

    static (float, float) ComputeRefraction(Intersection hit, Intersection[] intersections)
    {
        var containers = new List<Entity>();
        var from = RefractionThrough.Air;
        var to   = RefractionThrough.Air;
        
        foreach (var intersection in intersections)
        {
            if (intersection == hit
            && containers.Count != 0)
                from = containers.Last().Material.RefractionIndex;

            if (!containers.Remove(intersection.Collided))
                containers.Add(intersection.Collided);

            if (intersection == hit
            && containers.Count != 0)
            {
                to = containers.Last().Material.RefractionIndex;

                break;
            }
        }

        return ( from , to );
    }
    
    public static Precomputed Precompute(Intersection hit, Ray with, Intersection[]? others = null)
    {
        var rayTime  = hit.RayTime;
        var collided = hit.Collided;
        var point    = with.Position(rayTime);
        var eye      = -with.Direction;
        var normal   = hit.Collided.NormalAt(point);

        var normalEyeDot = normal.Dot(eye);
        var trueNormal   = normalEyeDot < 0 ? -normal : normal;
        var reflection   = with.Direction.Reflect(trueNormal);
        var overPoint    = point + trueNormal * Floating.Epsilon;
        var underPoint   = point - trueNormal * Floating.Epsilon;

        var (refractionFrom, refractionTo) = ComputeRefraction(hit, others ?? []);
        
        return Precomputed.Create(
              rayTime
            , collided
            , point
            , overPoint
            , underPoint
            , eye
            , trueNormal
            , reflection
            , refractionFrom
            , refractionTo
            , normalEyeDot < 0 
            );
    }
}
    