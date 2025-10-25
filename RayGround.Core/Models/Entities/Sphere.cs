using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Models.Entities;

namespace RayGround.Core;

public class Sphere : Entity
{
    Sphere(bool castShadows) : base(castShadows) { }

    public static Sphere Create(bool castShadows = true) =>
        new(castShadows);

    public static Sphere Glass() =>
        (Sphere)Create()
        .Paint(Material.Create(transparency: 1f, refractionIndex: 1.5f));

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
