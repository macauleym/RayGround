using RayGround.Core.Interfaces;
using RayGround.Core.Interfaces.Entities;
using RayGround.Core.Models;
using RayGround.Core.Models.Patterns;

namespace RayGround.Core;

public abstract class Entity
: IMorphable<Entity>
, IPaintable<Entity>
, IEtchable<Entity>
, IIntersectable
, INormalable
{
    public Fewple Origin { get; protected set; }
    public Matrix Transform { get; protected set; }
    public Material Material { get; protected set; }
    public Guid Id { get; protected set; }
    
    protected Entity(Fewple origin, Matrix? transform, Material? material, Guid? id)
    {
        Origin    = origin;
        Transform = transform ?? Matrix.Identity;
        Material  = material  ?? Material.Create();
        Id        = id        ?? Guid.NewGuid();
    }

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

    public Entity Etch(Pattern with)
    {
        Material = Material.Etch(with);

        return this;
    }

    public abstract float[] Intersections(Ray traced);

    public abstract Fewple LocalNormal(Fewple at);
}
