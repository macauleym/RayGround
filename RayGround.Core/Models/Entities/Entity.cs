using RayGround.Core.Interfaces;
using RayGround.Core.Interfaces.Entities;

namespace RayGround.Core.Models.Entities;

public abstract class Entity(bool castShadows) 
: IMorphable<Entity>
, IPaintable<Entity>
, IIntersectable
, INormalable
{
    public Guid Id { get; } = Guid.NewGuid();
    public Fewple Origin { get; } = Fewple.NewPoint(0, 0, 0);
    public Matrix Transform { get; protected set; } = Matrix.Identity;
    public Material Material { get; protected set; } = Material.Create();

    public bool CastShadows { get; protected set; } = castShadows;

    public Entity Morph(Matrix with)
    {
        Transform = with * Transform;

        return this;
    }

    public Entity Paint(Material target)
    {
        Material = target;

        return this;
    }
    
    public abstract float[] Intersections(Ray traced);

    public abstract Fewple LocalNormal(Fewple at);
}
