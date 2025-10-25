using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Operations;

namespace RayGround.Cli.Rendering.Blueprinting.Blueprints;

public class CameraView : IConstructable
{
    public Task<Blueprint> ConstructAsync(World into)
    {        
        // Spheres that will make up the scene.
        var baseSphere = Sphere.Create(); 
        
        var floor = baseSphere
            .Morph(Transform.Scaling(10 ,0.01f, 10))
            .Paint(Material
                .Create(specular: 0f)
                .Bucket(Color.Create(1,0.9f,0.9f)));
        var leftWall = floor
            .Morph(Transform.Translation(0, 0, 5))
            .Morph(Transform.RotationY(-float.Pi / 4))
            .Morph(Transform.RotationX(float.Pi / 2));
        var rightWall = floor
            .Morph(Transform.Translation(0, 0, 5))
            .Morph(Transform.RotationY(float.Pi / 4))
            .Morph(Transform.RotationX(float.Pi / 2));  

        var baseSphereMat = Material.Create(
              diffuse: 0.7f
            , specular: 0.3f
            );
        
        var centerSphere = baseSphere
            .Morph(Transform.Translation(-0.5f, 1, 0.5f))
            .Paint(baseSphereMat.Bucket(Color.Create(0.1f, 1, 0.5f)));
        var rightSphere = baseSphere
            .Morph(Transform.Translation(1.5f, 0.5f, 0.5f))
            .Morph(Transform.Scaling(0.5f, 0.5f, 0.5f))
            .Paint(baseSphereMat.Bucket(Color.Create(0.5f, 1, 0.1f)));
        var leftSphere = baseSphere
            .Morph(Transform.Translation(-1.5f, 0.33f, -0.75f))
            .Morph(Transform.Scaling(0.33f, 0.33f, 0.33f))
            .Paint(baseSphereMat.Bucket(Color.Create(1, 0.8f, 0.1f)));

        into.Lights.Add(Light.Create(Fewple.NewPoint(-10, 10, -10), Color.Create(1, 1, 1)));
        into.Entities.AddRange([
              floor
            , leftWall
            , rightWall
            , centerSphere
            , rightSphere
            , leftSphere
            ]);

        return Task.FromResult(Blueprint.Create(
              "camera-view"
            , into
            ));
    }
}
