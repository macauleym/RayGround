using RayGround.Core;
using RayGround.Core.Constants;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Models.Entities;
using RayGround.Core.Models.Patterns;
using RayGround.Core.Operations;

namespace RayGround.Cli.Rendering.Blueprinting.Blueprints;

public class GuysWithTheCube : IConstructable
{
    public Task<Blueprint> ConstructAsync(World into)
    {
        var floor = Core.Plane
            .Create()
            .Paint(Material.Create(specular: 0f)
                .Etch(Checker
                    .Create(Color.Create(0.5f, 0.8f, 0.2f), Color.Create(0.9f, 0.2f, 0.7f))
                    .Morph(Transform.Scaling(2, 2, 2))));

        var room = Cube
            .Create()
            .Morph(Transform.Scaling(15, 15, 15))
            .Paint(Material
                .Create(
                      diffuse: 0.8f
                    , specular: 0.1f
                    , shininess: 100)
                .Bucket(Color.Create(0.70f, 0.70f, 0.85f)));

        // Cube that will make up the scene.
        var diffuse  = 0.7f;
        var specular = 0.3f;

        var cube = Cube
            .Create()
            .Morph(Transform.RotationY(float.Pi / 6))
            .Morph(Transform.Translation(0, 1, 3))
            .Morph(Transform.Scaling(0.8f, 0.4f , 0.8f))
            .Paint(Material
                .Create(
                      ambient: 0.3f
                    , shininess: 250
                    , reflective: 0.1f)
                .Bucket(Color.Create(0.7f, 0.6f, 0.1f)));
        var prism = Cube
            .Create()
            .Morph(Transform.RotationY(-float.Pi / 8))
            .Morph(Transform.Translation(-4, 1, 0))
            .Morph(Transform.Scaling(0.5f, 0.5f, 0.5f))
            .Paint(Material
                .Create( 
                      ambient: 0.2f
                    , shininess: 400
                    , transparency: 0.9f
                    , refractionIndex: RefractionThrough.Diamond)
                .Bucket(Color.Create(0.3f, 0.1f, 0.1f)));
        var pillar = Cube
            .Create()
            .Morph(Transform.Translation(6, 1, 8))
            .Morph(Transform.Scaling(1, 3, 1))
            .Paint(Material
                .Create(
                      shininess: 300f
                    , reflective: 0.1f)
                .Etch(Gradient
                    .Default()
                    .Morph(Transform.Scaling(2, 1, 1))
                    .Morph(Transform.RotationZ(float.Pi / 2))
                    .Morph(Transform.Translation(-1, -1, -1))));
        
        into.Lights.Add(Light.Create(Fewple.NewPoint(-10, 10, -10), Color.Create(1, 1, 1)));
        into.Entities.AddRange([
              floor
            , room
            , cube
            , prism
            , pillar
            ]);
        
        return Task.FromResult(Blueprint.Create(
              "guys-with-the-cube"
            , into
            ));
    }
}
