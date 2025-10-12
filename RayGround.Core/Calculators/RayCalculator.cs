using RayGround.Core.Extensions;

namespace RayGround.Core.Calculators;

public static class RayCalculator
{
    public static RayTuple Position(Ray of, float t) =>
        of.Origin + of.Direction * t;

    public static Intersection[] Intersect(Ray of, Sphere withSphere)
    {
        var invRay      = of.Morph(withSphere.Transform.Inverse());
        var sphereToRay = invRay.Origin - RayTuple.NewPoint(0, 0, 0);//withSphere.Origin;
        
        var directionDot       = invRay.Direction.Dot(invRay.Direction);
        var sphereDirectionDot = 2.0f * invRay.Direction.Dot(sphereToRay);
        var sphereDot          = sphereToRay.Dot(sphereToRay) - 1.0f;
        
        var discriminant = MathF.Pow(sphereDirectionDot, 2) - 4 * directionDot * sphereDot;
        
        if (discriminant < 0)
            return [];

        var first  = (-sphereDirectionDot - MathF.Sqrt(discriminant)) / (2.0f * directionDot);
        var second = (-sphereDirectionDot + MathF.Sqrt(discriminant)) / (2.0f * directionDot);

        return [ Intersection.Create(first, withSphere)
               , Intersection.Create(second, withSphere)  
               ];
    }
}
