using RayGround.Cli.Logging;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Interfaces;
using RayGround.Core.Models;
using RayGround.Core.Operations;

namespace RayGround.Cli.Rendering;

public class PlanesRenderer(IExportCanvas exporter, ILogable logger) : Renderer(exporter, logger)
{
    async Task<Canvas> NeatlyOnThePlaneAsync()
    {
        var floor = Plane.Default()
            .Paint(Material.Create(
                  color: Color.Create(1, 0.9f, 0.9f)
                , specular: 0f
                ));

        var backWall = Plane.Default()
            .Paint(Material.Create(
                  color: Color.Create(0.6f, 0.6f, 1f)
                , specular: 0f))
            .Morph(Transform.RotationX(float.Pi / 2))
            .Morph(Transform.Translation(0f, 0f, 100));

        // Spheres that will make up the scene.
        var sphereMat = Material.Create(
              diffuse: 0.7f
            , specular: 0.3f
            );
        var centerSphere = Sphere.Unit()
            .Morph(Transform.Translation(-0.5f, 1, 0.5f))
            .Paint(sphereMat.Recolor(Color.Create(0.1f, 1, 0.5f)));
        var rightSphere = Sphere.Unit()
            .Morph(Transform.Translation(1.5f, 0.5f, 0.5f)
                   * Transform.Scaling(0.5f, 0.5f, 0.5f))
            .Paint(sphereMat.Recolor(Color.Create(0.5f, 1, 0.1f)));
        var leftSphere = Sphere.Unit()
            .Morph(Transform.Translation(-1.5f, 0.33f, -0.75f) * Transform.Scaling(0.33f, 0.33f, 0.33f))
            .Paint(sphereMat.Recolor(Color.Create(1, 0.8f, 0.1f)));

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
                  Fewple.NewPoint( 0, 1.5f, -5)
                , Fewple.NewPoint( 0,    1,  0)
                , Fewple.NewVector(0,    1,  0))
                );

        return camera.Render(world);
    }
    
    public override async Task RenderAsync()
    {
        var planeSceneCanvas = await NeatlyOnThePlaneAsync();
        await ExportCanvasAsync(planeSceneCanvas, "plane-scene.ppm");
    }
} 
