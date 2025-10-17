namespace RayGround.Core.Interfaces.Entities;

public interface IIntersectable
{
    public float[] Intersections(Ray traced);
}
