using RayGround.Core.Extensions;
using RayGround.Core.Operations;

namespace RayGround.Core;

public class World
{
    public readonly List<Light> Lights;
    public readonly List<Sphere> Shapes;

    World(List<Light>? lights, List<Sphere>? shapes)
    {
        Lights = lights ?? [];
        Shapes = shapes ?? [];
    }

    public static World Create(List<Light>? lights, List<Sphere>? shapes) =>
        new(lights, shapes);

    public static World Empty() => new(null, null);
    
    public static World Default()
    {
        var light   = Light.Create(RayTuple.NewPoint(-10, 10, -10), RayColor.Create(1, 1, 1));
        var sphere1 = Sphere.Create()
            .Paint(Material.Create(
                  diffuse: 0.7f
                , specular: 0.2f
                , color: RayColor.Create(0.8f, 1f, 0.6f)
                ));
        var sphere2 = Sphere.Create()
            .Morph(Transform.Scaling(0.5f, 0.5f, 0.5f));

        return Create([light], [sphere1, sphere2]);
    }
}
