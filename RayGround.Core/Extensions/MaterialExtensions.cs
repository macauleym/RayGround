namespace RayGround.Core.Extensions;

public static class MaterialExtensions
{
    public static Material Recolor(this Material source, RayColor with) =>
        Material.Create(
          source.Ambient
        , source.Diffuse
        , source.Specular
        , source.Shininess
        , with
        );
}
