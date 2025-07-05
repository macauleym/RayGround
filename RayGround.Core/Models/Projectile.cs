namespace RayGround.Core;

public class Projectile(RayTuple position, RayTuple velocity)
{
    public readonly RayTuple Position = position;
    public readonly RayTuple Velocity = velocity;
}
