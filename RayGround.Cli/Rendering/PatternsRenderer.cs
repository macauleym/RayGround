using RayGround.Cli.Logging;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Interfaces;
using RayGround.Core.Models;
using RayGround.Core.Models.Patterns;
using RayGround.Core.Operations;

namespace RayGround.Cli.Rendering;

public class PatternsRenderer(IExportCanvas exporter, ILogable logger) : Renderer(exporter, logger)
{
    Task<Canvas> PatternsAllAroundAsync() => Task.Run(() =>
    {
        var floor = Plane
            .Default()
            .Paint(Material.Create(specular: 0f))
            .Etch(Checker
                .Create(Color.Create(0.5f, 0.8f, 0.2f), Color.Create(0.9f, 0.2f, 0.7f))
                .Morph(Transform.Scaling(2, 2, 2)));

        var backWall = Plane
            .Default()
            .Paint(Material.Create(
                  color: Color.Create(0.6f, 0.6f, 1f)
                , specular: 0f))
            .Etch(Gradient
                .Create(Color.Create(0.1f, 0.1f, 0.5f), Color.Create(0.7f, 0.7f, 0.95f))
                .Morph(Transform.Scaling(22, 1, 1))
                .Morph(Transform.RotationY(float.Pi / 2)))
            .Morph(Transform.RotationX(float.Pi / 2))
            .Morph(Transform.Translation(0f, 0f, 100));

        // Spheres that will make up the scene.
        var diffuse  = 0.7f;
        var specular = 0.3f;
        
        var leftSphere = Sphere
            .Unit()
            .Morph(Transform.Scaling(0.33f, 0.33f, 0.33f))
            .Morph(Transform.Translation(-1.5f, 0.33f, -0.75f))
            .Paint(Material.Create(diffuse: diffuse, specular: specular))
            .Etch(Gradient
                .Create(Color.Create(1, 0.9f, 0.1f), Color.Create(1, 0.1f, 0.9f))
                .Morph(Transform.Scaling(2, 2, 2))
                .Morph(Transform.Translation(1, 0, 0))
                .Morph(Transform.RotationX(float.Pi / 2)));
        var centerSphere = Sphere.Unit()
            .Morph(Transform.Translation(-0.5f, 1, 0.5f))
            .Paint(Material
                .Create(diffuse: diffuse, specular: specular))
            .Etch(Stripe
                .Create(Color.Create(0.1f, 1, 0.5f), Color.Create(0.8f, 1, 0.8f)));
        var rightSphere = Sphere
            .Unit()
            .Morph(Transform.RotationX(float.Pi / 2))
            .Morph(Transform.RotationY(float.Pi / 6))
            .Morph(Transform.Scaling(0.5f, 0.5f, 0.5f))
            .Morph(Transform.Translation(1.5f, 0.5f, 0.5f))
            .Paint(Material.Create(diffuse: diffuse, specular: specular))
            .Etch(Rings
                .Create(Color.Create(0.5f, 1, 0.1f), Color.Create(1, 0.2f, 0.2f))
                .Morph(Transform.Scaling(.15f, 1, .15f)));

        // The world to put things in. 
        var world = World.Empty();
        world.Lights.Add(Light.Create(Fewple.NewPoint(-10, 10, -10), Color.Create(1, 1, 1)));
        world.Entities.AddRange([
              floor
            , backWall
            , leftSphere
            , centerSphere
            , rightSphere
            ]);

        // The camera that will render the scene viewport.
        var camera = Camera
            .Create(800, 400, float.Pi / 3)
            .Morph(View.Transform(
                  Fewple.NewPoint(0, 1.5f, -5)
                , Fewple.NewPoint(0, 1, 0)
                , Fewple.NewVector(0, 1, 0))
                );

        return camera.Render(world);
    });
    
    public override async Task RenderAsync()
    {
        var patternsCanvas = await PatternsAllAroundAsync();
        await ExportCanvasAsync(patternsCanvas, "patterns.ppm");
    }
}
