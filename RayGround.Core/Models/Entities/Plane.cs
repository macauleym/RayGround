using RayGround.Core.Constants;
using RayGround.Core.Models;
using RayGround.Core.Models.Entities;

namespace RayGround.Core;

public class Plane : Entity
{
    Plane(bool castShadows) : base(castShadows) { }

    public static Plane Create(bool castShadows = true) =>
        new(castShadows);

    public override float[] Intersections(Ray traced)
    {
        if (MathF.Abs(traced.Direction.Y) < Floating.Epsilon)
            return [];

        return [ -traced.Origin.Y / traced.Direction.Y ];
    }

    public override Fewple LocalNormal(Fewple at) =>
        Fewple.NewVector(0, 1, 0);
}
