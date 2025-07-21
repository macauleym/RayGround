using RayGround.Core.Extensions;

namespace RayGround.Core.Calculators;

public static class RayCalculator
{
    public static RayTuple Position(Ray of, float t) =>
        of.Origin + of.Direction * t;

    public static Intersection[] Intersect(Ray of, Sphere withSphere)
    {        
        var sphereToRay = of.Origin - withSphere.Origin;
        
        var directionDot       = of.Direction.Dot(of.Direction);
        var sphereDirectionDot = 2 * of.Direction.Dot(sphereToRay);
        var sphereDot          = sphereToRay.Dot(sphereToRay) - 1;
        
        var discriminant = MathF.Pow(sphereDirectionDot, 2) - 4 * directionDot * sphereDot;
        
        if (discriminant < 0)
            return [];

        var first  = (-sphereDirectionDot - MathF.Sqrt(discriminant)) / (2 * directionDot);
        var second = (-sphereDirectionDot + MathF.Sqrt(discriminant)) / (2 * directionDot);

        return [ Intersection.Create(first, withSphere)
               , Intersection.Create(second, withSphere)  
               ];
    }
}
