using RayGround.Core.Calculators;

namespace RayGround.Core.Extensions;

public static class RayExtensions
{
    public static RayTuple Position(this Ray of, float t) =>
        RayCalculator.Position(of, t);

    public static Intersection[] Intersect(this Ray of, Sphere withSphere) =>
        RayCalculator.Intersect(of, withSphere);

    public static Intersection[] IntersectWorld(this Ray of, List<Sphere> shapes) =>
        shapes
            .SelectMany(of.Intersect)
            .OrderBy(i => i.RayTime)
            .ToArray();

    public static Intersection[] IntersectWorld(this Ray of, World withWorld) =>
        of.IntersectWorld(withWorld.Shapes);
    
    public static Ray Morph(this Ray of, RayMatrix with) =>
        Ray.Create(
              with * of.Origin
            , with * of.Direction
            );
}
