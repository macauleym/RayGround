namespace RayGround.Core;

public class Sphere
{
    public readonly RayTuple Origin;
    public readonly float Radius;
    public readonly RayMatrix Transform;
    public readonly Material Material;
    public readonly Guid Id;

    Sphere(RayTuple origin, float radius, RayMatrix? transform, Material? material, Guid? id)
    {
        Origin    = origin;
        Radius    = radius;
        Transform = transform ?? RayMatrix.Identity;
        Material  = material  ?? Material.Create();
        Id        = id        ?? Guid.NewGuid();
    }

    public static Sphere Create(RayTuple origin, float radius, RayMatrix? transform, Material? material, Guid? id = null) =>
        new(origin, radius, transform, material, id);

    public static Sphere Create() =>
        new(RayTuple.NewPoint(0, 0, 0), 1f, null, null, null);
}
