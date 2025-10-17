using RayGround.Core.Models;

namespace RayGround.Core;

public class Projectile(Fewple position, Fewple velocity)
{
    public readonly Fewple Position = position;
    public readonly Fewple Velocity = velocity;
}
