using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Operations;

namespace RayGround.Cli.Rendering.Blueprinting.Blueprints;

public class Plane : IConstructable
{
    public Task<Blueprint> ConstructAsync(World into)
    {        
        var floor = Core.Plane
            .Create()
            .Paint(Material
                .Create(specular: 0f)
                .Bucket(Color.Create(1, 0.9f, 0.9f)));

        var backWall = Core.Plane
            .Create()
            .Paint(Material
                .Create(specular: 0f)
                .Bucket(Color.Create(0.6f, 0.6f, 1f)))
            .Morph(Transform.RotationX(float.Pi / 2))
            .Morph(Transform.Translation(0f, 0f, 100));

        // Spheres that will make up the scene.
        var sphereMat = Material.Create(
              diffuse: 0.7f
            , specular: 0.3f
            );
        var centerSphere = Sphere.Create()
            .Morph(Transform.Translation(-0.5f, 1, 0.5f))
            .Paint(sphereMat.Bucket(Color.Create(0.1f, 1, 0.5f)));
        var rightSphere = Sphere.Create()
            .Morph(Transform.Translation(1.5f, 0.5f, 0.5f)
                   * Transform.Scaling(0.5f, 0.5f, 0.5f))
            .Paint(sphereMat.Bucket(Color.Create(0.5f, 1, 0.1f)));
        var leftSphere = Sphere.Create()
            .Morph(Transform.Translation(-1.5f, 0.33f, -0.75f) * Transform.Scaling(0.33f, 0.33f, 0.33f))
            .Paint(sphereMat.Bucket(Color.Create(1, 0.8f, 0.1f)));

        into.Lights.Add(Light.Create(Fewple.NewPoint(-10, 10, -10), Color.Create(1, 1, 1)));
        into.Entities.AddRange([
              floor
            , backWall
            , leftSphere
            , centerSphere
            , rightSphere
            ]);

        return Task.FromResult(Blueprint.Create(
              "plane-scene"
            , into
            ));
    }
} 
