using RayGround.Core.Models;

namespace RayGround.Core;

public class Light
{
    public readonly Fewple Position;
    public readonly RayColor Intensity;

    Light(Fewple position, RayColor intensity)
    {
        Position  = position;
        Intensity = intensity;
    }

    public static Light Create(Fewple position, RayColor intensity) =>
        new(position, intensity);
}
