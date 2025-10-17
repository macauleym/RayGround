using RayGround.Core.Extensions;
using RayGround.Core.Models;

namespace RayGround.Core;

public class Sphere : Entity
{
    protected Sphere(Fewple origin, Matrix? transform, Material? material, Guid? id)
    : base(origin, transform, material, id)
    { }

    public static Sphere Create(Fewple origin,Matrix? transform, Material? material, Guid? id = null) =>
        new(origin, transform, material, id);

    public static Sphere Unit() =>
        new(Fewple.NewPoint(0, 0, 0), null, null, null);

    public override float[] Intersections(Ray traced)
    {       
        var sphereToRay = traced.Origin - Origin;
        
        var directionDot       = traced.Direction.Dot(traced.Direction);
        var sphereDirectionDot = 2.0f * traced.Direction.Dot(sphereToRay);
        var sphereDot          = sphereToRay.Dot(sphereToRay) - 1.0f;
        
        var discriminant = MathF.Pow(sphereDirectionDot, 2) - 4 * directionDot * sphereDot;
        
        if (discriminant < 0)
            return [];

        var first  = (-sphereDirectionDot - MathF.Sqrt(discriminant)) / (2.0f * directionDot);
        var second = (-sphereDirectionDot + MathF.Sqrt(discriminant)) / (2.0f * directionDot);

        return [first, second];
    }

    public override Fewple LocalNormal(Fewple at) =>
        at - Fewple.NewPoint(0, 0, 0);
}
