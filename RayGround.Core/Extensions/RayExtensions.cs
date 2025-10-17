using RayGround.Core.Calculators;
using RayGround.Core.Models;

namespace RayGround.Core.Extensions;

public static class RayExtensions
{
    public static Fewple Position(this Ray of, float t) =>
        RayCalculator.Position(of, t);

    public static Intersection[] Intersect(this Ray of, Entity withEntity) =>
        RayCalculator.Intersect(of, withEntity);

    public static Intersection[] IntersectWorld(this Ray of, List<Entity> entities) =>
        entities
            .SelectMany(of.Intersect)
            .OrderBy(i => i.RayTime)
            .ToArray();

    public static Intersection[] IntersectWorld(this Ray of, World withWorld) =>
        of.IntersectWorld(withWorld.Entities);
    
    public static Ray Morph(this Ray of, Matrix with) =>
        Ray.Create(
              with * of.Origin
            , with * of.Direction
            );
}
