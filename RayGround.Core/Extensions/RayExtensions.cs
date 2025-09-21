using RayGround.Core.Calculators;

namespace RayGround.Core.Extensions;

public static class RayExtensions
{
    public static RayTuple Position(this Ray of, float t) =>
        RayCalculator.Position(of, t);

    public static Intersection[] Intersect(this Ray of, Sphere withSphere) =>
        RayCalculator.Intersect(
              of.Morph(
                withSphere.Transform.Inverse())
            , withSphere
            );

    public static Intersection[] IntersectWorld(this Ray of, World withWorld) =>
        withWorld.Shapes
            .SelectMany(shape => 
                RayCalculator.Intersect(of.Morph(shape.Transform.Inverse()), shape))
            .OrderBy(i => i.RayPoint)
            .ToArray();
    
    public static Ray Morph(this Ray of, RayMatrix with) =>
        Ray.Create((with * of.Origin).ToTuple(), (with * of.Direction).ToTuple());
}
