using RayGround.Core.Extensions;
using RayGround.Core.Operations;

namespace RayGround.Core.Calculators;

public static class WorldCalculator
{
    public static Color ShadeHit(World world, Precomputed computations) =>
        world.Lights.Aggregate(Color.Create(0, 0, 0), (color, light) =>
            color +
            Illuminate.Lighting(
              computations.Collided.Material
            , light
            , computations.Point
            , computations.EyeVector
            , computations.NormalVector
            , Illuminate.IsShadowed(light, computations.OverPoint, world.Entities)
            ));

    public static Color ColorAt(World world, Ray ray)
    {
        var intersections = ray.IntersectWorld(world);
        var hit           = intersections.Hit();
        if (hit is null)
            return Color.Black;

        var computations = hit.Value.Precompute(ray);
        
        return ShadeHit(world, computations);
    }
}
