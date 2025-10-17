namespace RayGround.Core;

public struct Intersection
{
    public readonly float RayTime;
    public readonly Entity Collided;

    Intersection(float rayTime, Entity collided)
    {
        RayTime  = rayTime;
        Collided = collided;
    }

    public static Intersection Create(float at, Entity collided) =>
        new(at, collided);
}
