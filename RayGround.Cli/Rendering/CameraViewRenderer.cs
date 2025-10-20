using RayGround.Cli.Logging;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Interfaces;
using RayGround.Core.Models;
using RayGround.Core.Operations;

namespace RayGround.Cli.Rendering;

public class CameraViewRenderer(IExportCanvas exporter, ILogable logger) : Renderer(exporter, logger)
{
    async Task<Canvas> RenderCameraSceneAsync()
    {
        // Spheres that will make up the scene.
        var baseSphere = Sphere.Unit(); 
        
        var floor = baseSphere
            .Morph(Transform.Scaling(10 ,0.01f, 10))
            .Paint(Material.Create(
                  color: Color.Create(1,0.9f,0.9f)
                , specular: 0f)
                );
        var leftWall = floor
            .Morph(Transform.Translation(0, 0, 5)
                   * Transform.RotationY(-float.Pi / 4)
                   * Transform.RotationX(float.Pi / 2));
        var rightWall = floor
            .Morph(Transform.Translation(0, 0, 5)
                   * Transform.RotationY(float.Pi / 4)
                   * Transform.RotationX(float.Pi / 2));

        var baseSphereMat = Material.Create(
              diffuse: 0.7f
            , specular: 0.3f
            );
        var centerSphere = baseSphere
            .Morph(Transform.Translation(-0.5f, 1, 0.5f))
            .Paint(baseSphereMat.Recolor(Color.Create(0.1f, 1, 0.5f)));
        var rightSphere = baseSphere
            .Morph(Transform.Translation(1.5f, 0.5f, 0.5f)
                   * Transform.Scaling(0.5f, 0.5f, 0.5f))
            .Paint(baseSphereMat.Recolor(Color.Create(0.5f, 1, 0.1f)));
        var leftSphere = baseSphere
            .Morph(Transform.Translation(-1.5f, 0.33f, -0.75f) * Transform.Scaling(0.33f, 0.33f, 0.33f))
            .Paint(baseSphereMat.Recolor(Color.Create(1, 0.8f, 0.1f)));

        // The world to put things in. 
        var world = World.Empty();
        world.Lights.Add(Light.Create(Fewple.NewPoint(-10, 10, -10), Color.Create(1, 1, 1)));
        world.Entities.AddRange([
              floor
            , leftWall
            , rightWall
            , centerSphere
            , rightSphere
            , leftSphere
            ]);

        // The camera that will render the scene viewport.
        var camera = Camera
            .Create(800, 400, float.Pi / 3)
            //.Create(600, 200, float.Pi / 3)
            .Morph(View.Transform(
              Fewple.NewPoint( 0, 1.5f, -5)
            , Fewple.NewPoint( 0,    1,  0)
            , Fewple.NewVector(0,    1,  0))
            );

        return camera.Render(world);
    }
    
    public override async Task RenderAsync()
    {
        var cameraViewCanvas = await RenderCameraSceneAsync();
        await ExportCanvasAsync(cameraViewCanvas, "camera-view.ppm");
    }
}
