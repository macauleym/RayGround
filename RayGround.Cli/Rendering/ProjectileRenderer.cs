using RayGround.Cli.Logging;
using RayGround.Core;
using RayGround.Core.Calculators;
using RayGround.Core.Extensions;
using RayGround.Core.Interfaces;
using RayGround.Core.Models;
using Environment = RayGround.Core.Environment;

namespace RayGround.Cli.Rendering;

public class ProjectileRenderer(IStrokable brush, IExportCanvas exporter, ILogable logger) : Renderer(exporter, logger)
{
    async Task<Canvas> LaunchProjectileAsync()
    {
        Logger.Log("Prep the projectile...");
        var start      = Fewple.NewPoint(0, 1, 0);
        var velocity   = Fewple.NewVector(1, 1.8f, 0).Normalize() * 11.25f;
        var projectile = new Projectile(start, velocity);

        Logger.Log("Get the environment...");
        var gravity     = Fewple.NewVector(0, -0.1f, 0);
        var wind        = Fewple.NewVector(-0.01f, 0, 0);
        var environment = new Environment(gravity, wind);
        var calculator  = new EnvironmentCalculator();

        Logger.Log("Build the canvas...");
        var canvas          = new Canvas(900, 550);
        var projectileColor = Color.Create(0.8f, 0.8f, 0.8f);

        Logger.Log($"Projectile starting at {projectile.Position}.");
        brush.Stroke(canvas, projectile.Position, projectileColor);
        Logger.Log("Aim...");
        Logger.Log("Fire!");
        while (projectile.Position.Y > 0)
        {
            projectile = calculator.Tick(projectile, environment);
            Logger.Log(projectile.Position.ToString());
            brush.Stroke(canvas, projectile.Position, projectileColor);
        }

        Logger.Log("Landed!");
        Logger.LogSeparator();

        return canvas;
    }

    public override async Task RenderAsync()
    {
        var projectileCanvas = await LaunchProjectileAsync();
        await ExportCanvasAsync(projectileCanvas, "projectile.ppm");
    }
}
