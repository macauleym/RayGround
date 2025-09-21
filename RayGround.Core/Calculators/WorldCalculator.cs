using RayGround.Core.Extensions;
using RayGround.Core.Operations;

namespace RayGround.Core.Calculators;

public static class WorldCalculator
{
    public static RayColor ShadeHit(World world, Precomputed computations) =>
        // TODO: To support multiple lights, call Lighting on each light source
        // and add all the colors together.
        Illuminate.Lighting(
              computations.Collided.Material
            , world.Lights.First()
            , computations.Point
            , computations.EyeVector
            , computations.NormalVector
            );

    public static RayColor ColorAt(World world, Ray ray)
    {
        var intersections = ray.IntersectWorld(world);
        var hit           = intersections.Hit();
        if (hit is null)
            return RayColor.Black;

        var computations = hit.Value.Precompute(ray);
        
        return ShadeHit(world, computations);
    }
}
