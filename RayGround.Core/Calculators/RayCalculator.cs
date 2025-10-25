using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Models.Entities;

namespace RayGround.Core.Calculators;

public static class RayCalculator
{
    public static Fewple Position(Ray of, float t) =>
        of.Origin + of.Direction * t;

    public static Intersection[] Intersect(Ray of, Entity withEntity) =>
        withEntity
        .Intersections(withEntity.BindRay(of))
        .Select(p => Intersection.Create(p, withEntity))
        .ToArray();
}
