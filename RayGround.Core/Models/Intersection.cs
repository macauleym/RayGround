namespace RayGround.Core;

public struct Intersection
{
    public readonly float RayTime;
    public readonly object Collided;

    Intersection(float rayTime, object collided)
    {
        RayTime  = rayTime;
        Collided = collided;
    }

    public static Intersection Create(float at, object collided) =>
        new(at, collided);
}
