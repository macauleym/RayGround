namespace RayGround.Core.Handlers;

public class EnvironmentCalculator
{
    public Projectile Tick(Projectile forProjectile, Environment withEnvironment)
    {
        var nextPosition = forProjectile.Position + forProjectile.Velocity;
        var nextVelocity = forProjectile.Velocity + withEnvironment.Gravity + withEnvironment.Wind;

        return new(nextPosition, nextVelocity);
    }
}
