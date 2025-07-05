namespace RayGround.Core.Handlers;

public class EnvironmentCalculator
{
    public Projectile Tick(Projectile forProjectile, Environment inEnvironment)
    {
        var nextPosition = forProjectile.Position + forProjectile.Velocity;
        var nextVelocity = forProjectile.Velocity + inEnvironment.Gravity + inEnvironment.Wind;

        return new(nextPosition, nextVelocity);
    }
}
