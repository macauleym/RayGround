using RayGround.Core.Models;

namespace RayGround.Core.Extensions;

public static class EntityExtensions
{
    public static Ray BindRay(this Entity source, Ray traced) =>
        traced.Morph(source.Transform.Inverse());
    
    public static Fewple NormalAt(this Entity source, Fewple at)
    {
        var inversed     = source.Transform.Inverse();
        var entityPoint  = inversed * at;
        var entityNormal = source.LocalNormal(entityPoint);
        var worldNormal  = inversed.Transpose() * entityNormal; 

        return Fewple
            .NewVector(worldNormal.X, worldNormal.Y, worldNormal.Z)
            .Normalize();
    }
}
