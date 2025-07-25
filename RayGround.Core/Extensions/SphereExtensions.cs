namespace RayGround.Core.Extensions;

public static class SphereExtensions
{
    public static Sphere UpdateTransform(this Sphere source, RayMatrix with) =>
        Sphere.Create(
              source.Origin
            , source.Radius
            , with * source.Transform
            , source.Material
            , source.Id
            );

    public static Sphere UpdateMaterial(this Sphere source, Material target) =>
        Sphere.Create(
              source.Origin
            , source.Radius
            , source.Transform
            , target
            , source.Id
            );
    
    public static RayTuple NormalAt(this Sphere source, RayTuple at)
    {
        var objPoint    = (source.Transform.Inverse() * at).ToTuple();
        var objNormal   = objPoint - RayTuple.NewPoint(0, 0, 0);
        var worldNormal = (source.Transform.Inverse().Transpose() * objNormal).ToTuple(); 

        return RayTuple
            .NewVector(worldNormal.X, worldNormal.Y, worldNormal.Z)
            .Normalize();
    }
}
