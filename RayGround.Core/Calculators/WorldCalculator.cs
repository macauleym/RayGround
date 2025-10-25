using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Operations;

namespace RayGround.Core.Calculators;

public static class WorldCalculator
{
    public static Color ShadeHit(World world, Precomputed computations, int remaining = 7)
    {
        var surface = world.Lights.Aggregate(Color.Black, (color, surface) =>
            color
            + Illuminate.Lighting(
              computations.Collided.Material
            , computations.Collided
            , surface
            , computations.Point
            , computations.EyeVector
            , computations.NormalVector
            , Illuminate.IsShadowed(surface, computations.OverPoint, world.Entities)
            )
        );
        
        var reflected = world.ReflectedColor(computations, remaining); 
        var refracted = world.RefractedColor(computations, remaining);
        
        if (computations.Collided.Material is not { Reflective: > 0, Transparency: > 0 })
           return surface + reflected + refracted;
        
        var reflectance = computations.Schlick();
        
        return surface 
               + reflected * reflectance 
               + refracted * (1 - reflectance);
    }

    public static Color ColorAt(World world, Ray ray, int remaining = 7)
    {
        var intersections = ray.IntersectWorld(world);
        var hit           = intersections.Hit();
        if (hit is null)
            return Color.Black;

        var computations = hit.Precompute(ray, intersections);
        
        return ShadeHit(world, computations, remaining);
    }

    public static Color ReflectedColor(World world, Precomputed precomputed, int remaining = 7)
    {
        if (precomputed.Collided.Material.Reflective.NearlyEqual(0f)
        || remaining <= 0)
            return Color.Black;

        var reflection = Ray.Create(precomputed.OverPoint, precomputed.ReflectVector);
        var reflected  = world.ColorAt(reflection, remaining - 1) 
                         * precomputed.Collided.Material.Reflective;

        return reflected;
    }
    
    public static Color RefractedColor(this World world, Precomputed precomputed, int remaining = 7)
    {
        if (precomputed.Collided.Material.Transparency.NearlyEqual(0)
        || remaining == 0)
            return Color.Black;
        
        // Find the ratio of the first refraction to the second.
        var refractionRatio = precomputed.RefractionFrom / precomputed.RefractionTo;
        
        // cos(theta_i) is the same as taking the dot product between vectors.
        var cosI = precomputed.EyeVector.Dot(precomputed.NormalVector);
        
        // Now find sin(theta_t)^2 via the trig identity.
        var sin2T = MathF.Pow(refractionRatio, 2f) * (1f - MathF.Pow(cosI, 2f));
        if (sin2T > 1f)
            return Color.Black;
        
        // Find cos(theta_t) from the trig identity.
        var cosT = MathF.Sqrt(1f - sin2T);
        
        // Compute refraction direction.
        var direction = precomputed.NormalVector 
                        * (refractionRatio * cosI - cosT) 
                        - precomputed.EyeVector 
                        * refractionRatio;

        var refraction = Ray.Create(precomputed.UnderPoint, direction);
        var refracted  = world.ColorAt(refraction, remaining - 1) 
                         * precomputed.Collided.Material.Transparency;

        return refracted;
    }
}
