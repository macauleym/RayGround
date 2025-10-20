using RayGround.Core.Models;
using RayGround.Core.Models.Patterns;

namespace RayGround.Core.Extensions;

public static class PatternExtensions
{
    public static Color GetLocalizedColor(this Pattern source, Matrix localTransform, Fewple worldPoint) =>
        source.GetColor(source.Localize(localTransform, worldPoint));
}
