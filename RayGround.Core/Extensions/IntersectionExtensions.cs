using RayGround.Core.Calculators;

namespace RayGround.Core.Extensions;

public static class IntersectionExtensions
{
    public static Intersection? Hit(this Intersection[] source) =>
        IntersectionCalculator.Hit(source);
}
