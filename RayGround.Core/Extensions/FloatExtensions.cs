using RayGround.Core.Constants;

namespace RayGround.Core.Extensions;

public static class FloatExtensions
{
    public static bool NearlyEqual(this float source, float other)
    {
        var diff = Math.Abs(source - other);

        return diff < Floating.Epsilon;
    }
}
