using RayGround.Core.Calculators;
using RayGround.Core.Models;

namespace RayGround.Core.Extensions;

public static class IntersectionExtensions
{
    public static Intersection? Hit(this Intersection[] source) =>
        IntersectionCalculator.Hit(source);

    public static Precomputed Precompute(this Intersection source, Ray with, Intersection[]? others = null) =>
        IntersectionCalculator.Precompute(source, with, others ?? [source]);
}
