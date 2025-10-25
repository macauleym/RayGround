using RayGround.Core.Constants;
using RayGround.Core.Extensions;

namespace RayGround.Core.Models.Entities;

public class Cube : Entity
{
    Cube(bool castShadows) : base(castShadows) { }

    public static Cube Create(bool castShadows = true) =>
        new(castShadows);

    (float, float) CheckAxis(float origin, float direction)
    {
        var minNumerator = -1 - origin;
        var maxNumerator =  1 - origin;

        var min = GetIntersect(minNumerator);
        var max = GetIntersect(maxNumerator);

        return min > max 
            ? (max, min) 
            : (min, max);

        float GetIntersect(float num) =>
            MathF.Abs(direction) >= Floating.Epsilon
                ? num / direction
                : num * Floating.Infinity;
    }
    
    public override float[] Intersections(Ray traced)
    {
        /* TODO
         * We can shorten this check; we don't need to check every axis every time.
         * How can we short-circuit to terminate early if we know the
         * intersection misses early?
         * Need to think about this a bit more. Research is required. 
         */
        var (xMin, xMax) = CheckAxis(traced.Origin.X, traced.Direction.X);
        var (yMin, yMax) = CheckAxis(traced.Origin.Y, traced.Direction.Y);
        var (zMin, zMax) = CheckAxis(traced.Origin.Z, traced.Direction.Z);

        var tMin = new[] { xMin, yMin, zMin }.Max();
        var tMax = new[] { xMax, yMax, zMax }.Min();

        return tMin > tMax ? [] : [tMin, tMax];
    }

    public override Fewple LocalNormal(Fewple at)
    {
        var maxC = new[] { at.X, at.Y, at.Z }.Select(MathF.Abs).Max();
        if (maxC.NearlyEqual(MathF.Abs(at.X)))
            return Fewple.NewVector(at.X ,    0 ,    0);
        if (maxC.NearlyEqual(MathF.Abs(at.Y)))
            return Fewple.NewVector(   0 , at.Y ,    0);
        if (maxC.NearlyEqual(MathF.Abs(at.Z)))
            return Fewple.NewVector(   0 ,    0 , at.Z);

        return Fewple.NewVector(0, 0, 0);
    }
}
