namespace RayGround.Core.Calculators;

public static class IntersectionCalculator
{
    public static Intersection? Hit(Intersection[] intersections)
    {
        var xs = intersections
            .Where(i => i.RayPoint >= 0)
            .OrderBy(i => i.RayPoint)
            .ToArray();
        
        return xs.Length > 0 
            ? xs.First() 
            : null;
    }
}
