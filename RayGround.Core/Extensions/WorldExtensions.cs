using RayGround.Core.Calculators;

namespace RayGround.Core.Extensions;

public static class WorldExtensions
{
    public static Color ShadeHit(this World source, Precomputed computations) =>
        WorldCalculator.ShadeHit(source, computations);

    public static Color ColorAt(this World source, Ray ray) =>
        WorldCalculator.ColorAt(source, ray);
}
