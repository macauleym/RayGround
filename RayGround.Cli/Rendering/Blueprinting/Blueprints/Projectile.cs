using RayGround.Cli.Logging;
using RayGround.Core;
using RayGround.Core.Calculators;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Models.Entities;
using RayGround.Core.Operations;
using Environment = RayGround.Core.Environment;

namespace RayGround.Cli.Rendering.Blueprinting.Blueprints;

public class Projectile(ILogable logger) : IConstructable
{
    public Task<Blueprint> ConstructAsync(World into)
    {
        logger.Log("Prep the projectile...");
        var start      = Fewple.NewPoint(0, 1, 0);
        var velocity   = Fewple.NewVector(1, 1.8f, 0).Normalize() * 11.25f;
        var projectile = new Core.Projectile(start, velocity);

        logger.Log("Get the environment...");
        var gravity     = Fewple.NewVector(0, -0.1f, 0);
        var wind        = Fewple.NewVector(-0.01f, 0, 0);
        var environment = new Environment(gravity, wind);
        var calculator  = new EnvironmentCalculator();

        var projectileMaterial = Material.Create().Bucket(Color.Create(0.8f, 0.8f, 0.8f));
        var projectileScale    = Transform.Scaling(0.1f, 0.1f, 0.1f);

        logger.Log($"Projectile starting at {projectile.Position}.");
        var spheres = new List<Entity> { Sphere
                .Create()
                .Morph(Transform.Translation(
                      projectile.Position.X
                    , projectile.Position.Y
                    , projectile.Position.Z
                    ))
                .Morph(projectileScale)
                .Paint(projectileMaterial)
            };
        
        logger.Log("Aim...");
        logger.Log("Fire!");
        while (projectile.Position.Y > 0)
        {
            projectile = calculator.Tick(projectile, environment);
            logger.Log(projectile.Position.ToString());
            spheres.Add(Sphere
                .Create()
                .Morph(Transform.Translation(
                      projectile.Position.X
                    , projectile.Position.Y
                    , projectile.Position.Z
                    ))
                .Morph(projectileScale)
                .Paint(projectileMaterial)
                );
        }

        into.Lights.Add(Light.Create(Fewple.NewPoint(0, 0, -10), Color.White));
        into.Entities.AddRange(spheres);
        
        logger.Log("Landed!");
        logger.LogSeparator();
        
        return Task.FromResult(Blueprint.Create(
              "projectile"
            , into
            ));
    }
}
