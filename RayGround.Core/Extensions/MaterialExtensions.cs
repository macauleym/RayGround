using RayGround.Core.Models.Patterns;

namespace RayGround.Core.Extensions;

public static class MaterialExtensions
{
    public static Material Bucket(this Material source, Color spill) =>
        source.Etch(SolidPattern.Create(spill));
}
