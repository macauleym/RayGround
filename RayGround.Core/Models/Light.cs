namespace RayGround.Core;

public class Light
{
    public readonly RayTuple Position;
    public readonly RayColor Intensity;

    Light(RayTuple position, RayColor intensity)
    {
        Position  = position;
        Intensity = intensity;
    }

    public static Light Create(RayTuple position, RayColor intensity) =>
        new(position, intensity);
}
