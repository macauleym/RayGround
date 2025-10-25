using RayGround.Core.Calculators;
using RayGround.Core.Models;

namespace RayGround.Core.Extensions;

public static class WorldExtensions
{
    public static Color ShadeHit(this World source, Precomputed computations, int remaining = 7) =>
        WorldCalculator.ShadeHit(source, computations, remaining);

    public static Color ColorAt(this World source, Ray ray, int remaining = 7) =>
        WorldCalculator.ColorAt(source, ray, remaining);

    public static Color ReflectedColor(this World source, Precomputed precomputed, int remaining = 7) =>
        WorldCalculator.ReflectedColor(source, precomputed, remaining);

    public static Color RefractedColor(this World source, Precomputed precomputed, int remaining = 7) =>
        WorldCalculator.RefractedColor(source, precomputed, remaining);
}
