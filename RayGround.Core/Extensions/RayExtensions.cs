using RayGround.Core.Calculators;

namespace RayGround.Core.Extensions;

public static class RayExtensions
{
    public static RayTuple Position(this Ray of, float t) =>
        RayCalculator.Position(of, t);

    public static Intersection[] Intersect(this Ray of, Sphere withSphere) =>
        RayCalculator.Intersect(
              of.Transform(
                withSphere.Transform.Inverse())
            , withSphere
            );

    public static Ray Transform(this Ray of, RayMatrix with) =>
        Ray.Create((with * of.Origin).ToTuple(), (with * of.Direction).ToTuple());
}
