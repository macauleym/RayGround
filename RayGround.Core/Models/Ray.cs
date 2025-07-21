namespace RayGround.Core;

public class Ray
{
    public readonly RayTuple Origin;
    public readonly RayTuple Direction;

    Ray(RayTuple origin, RayTuple direction)
    {
        Origin    = origin;
        Direction = direction;
    }

    public static Ray Create(RayTuple origin, RayTuple direction) =>
        new (origin, direction);
}
