using RayGround.Core;
using RayGround.Core.Constants;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Models.Patterns;
using RayGround.Core.Operations;

namespace RayGround.Cli.Rendering.Blueprinting.Blueprints;

public class ReflectionRefraction : IConstructable
{
    public Task<Blueprint> ConstructAsync(World into)
    {
        var floor = Core.Plane
            .Create()
            .Paint(Material.Create(specular: 0f)
                .Etch(Checker
                    .Create(Color.Create(0.5f, 0.8f, 0.2f), Color.Create(0.9f, 0.2f, 0.7f))
                    .Morph(Transform.Scaling(2, 2, 2))));

        var backWall = Core.Plane
            .Create()
            .Paint(Material
                .Create(specular: 0f)
                .Etch(Gradient
                    .Create(Color.Create(0.70f, 0.70f, 0.85f), Color.Create(0.40f, 0.40f, 0.70f))
                    .Morph(Transform.Scaling(35, 1, 1))
                    .Morph(Transform.RotationY(float.Pi / 2))))
            .Morph(Transform.RotationX(float.Pi / 2))
            .Morph(Transform.Translation(0f, 0f, 25));
        var frontWall = Core.Plane
            .Create()
            .Paint(Material
                .Create(specular: 0f)
                .Etch(Gradient
                    .Create(Color.Create(0.85f, 0.85f, 0.7f), Color.Create(0.6f, 0.6f, 0.4f))
                    .Morph(Transform.Scaling(35, 1, 1))
                    .Morph(Transform.RotationY(float.Pi / 2))))
            .Morph(Transform.RotationX(-float.Pi / 2))
            .Morph(Transform.Translation(0f, 0f, -25));

        // Spheres that will make up the scene.
        var diffuse  = 0.7f;
        var specular = 0.3f;
        
        var leftSphere = Sphere
            .Create()
            .Morph(Transform.Scaling(0.33f, 0.33f, 0.33f))
            .Morph(Transform.Translation(-1.5f, 0.33f, -0.75f))
            .Paint(Material
                .Create(diffuse: diffuse, specular: specular)
                .Etch(Gradient
                    .Create(Color.Create(1, 0.9f, 0.1f), Color.Create(1, 0.1f, 0.9f))
                    .Morph(Transform.Scaling(2, 2, 2))
                    .Morph(Transform.Translation(1, 0, 0))
                    .Morph(Transform.RotationX(float.Pi / 2))));
        var centerSphere = Sphere
            .Create()
            .Morph(Transform.Translation(-0.5f, 1, 0.5f))
            .Paint(Material
                .Create(
                  diffuse: 0.3f
                , shininess: 400f
                , transparency: 0.85f
                , refractionIndex: RefractionThrough.Glass)
                .Bucket(Color.Create(0.02f, 0.1f, 0.04f)));
        var offCenterSphere = Sphere
            .Create()
            .Morph(Transform.Translation(1, 1, 5))
            .Paint(Material
                .Create(
                  diffuse: 0.2f
                , specular: 0.2f
                , shininess: 400f
                , reflective: 0.9f)
                .Bucket(Color.Create(0.02f, 0.1f, 0.04f)));
        var rightSphere = Sphere
            .Create()
            .Morph(Transform.RotationX(float.Pi / 2))
            .Morph(Transform.RotationY(float.Pi / 6))
            .Morph(Transform.Scaling(0.5f, 0.5f, 0.5f))
            .Morph(Transform.Translation(1.5f, 0.5f, 0.5f))
            .Paint(Material
                .Create(diffuse: diffuse, specular: specular)
                .Etch(Rings
                    .Create(Color.Create(0.5f, 1, 0.1f), Color.Create(1, 0.2f, 0.2f))
                    .Morph(Transform.Scaling(.15f, 1, .15f))));

        into.Lights.Add(Light.Create(Fewple.NewPoint(-10, 10, -10), Color.Create(1, 1, 1)));
        into.Entities.AddRange([
              floor
            , backWall
            , leftSphere
            , centerSphere
            , offCenterSphere
            , rightSphere
            , frontWall
            ]);
        
        return Task.FromResult(Blueprint.Create(
              "reflection-refraction"
            , into
            ));
    }
}
