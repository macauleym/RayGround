namespace RayGround.Core;

public struct Intersection
{
    public readonly float RayPoint;
    public readonly object Collided;

    Intersection(float rayPoint, object collided)
    {
        RayPoint = rayPoint;
        Collided = collided;
    }

    public static Intersection Create(float at, object collided) =>
        new(at, collided);
}
