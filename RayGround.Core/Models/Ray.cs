using RayGround.Core.Models;

namespace RayGround.Core;

public class Ray
{
    public readonly Fewple Origin;
    public readonly Fewple Direction;

    Ray(Fewple origin, Fewple direction)
    {
        Origin    = origin;
        Direction = direction;
    }

    public static Ray Create(Fewple origin, Fewple direction) =>
        new (origin, direction);
}
