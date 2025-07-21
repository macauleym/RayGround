namespace RayGround.Core.Extensions;

public static class SphereExtensions
{
    public static Sphere UpdateTransform(this Sphere source, RayMatrix with) =>
        Sphere.Create(
              source.Origin
            , source.Radius
            , with * source.Transform
            , source.Id
            );
}
