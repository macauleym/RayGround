using RayGround.Core.Constants;
using RayGround.Core.Models;

namespace RayGround.Core;

public class Plane : Entity
{
    protected Plane(Fewple origin, Matrix? transform, Material? material, Guid? id) 
    : base(origin, transform, material, id)
    { }

    public static Plane Create(Fewple origin, Matrix? transform, Material? material, Guid? id) =>
        new(origin, transform, material, id);

    public static Plane Default() =>
        Create(
              Fewple.NewPoint(0, 0, 0)
            , Matrix.Identity
            , Material.Create()
            , Guid.NewGuid()
            );
    
    public override float[] Intersections(Ray traced)
    {
        if (MathF.Abs(traced.Direction.Y) < Floating.Epsilon)
            return [];

        return [ -traced.Origin.Y / traced.Direction.Y ];
    }

    public override Fewple LocalNormal(Fewple at) =>
        Fewple.NewVector(0, 1, 0);
}
