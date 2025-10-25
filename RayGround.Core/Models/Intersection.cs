using RayGround.Core.Models.Entities;

namespace RayGround.Core;

public class Intersection
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

    /*public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is null)
            return false;

        var other = (Intersection)obj;

        return RayTime.NearlyEqual(other.RayTime)
               && Collided == other.Collided;
    }*/
}
