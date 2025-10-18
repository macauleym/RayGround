using RayGround.Core.Models;

namespace RayGround.Core;

public class Light
{
    public readonly Fewple Position;
    public readonly Color Intensity;

    Light(Fewple position, Color intensity)
    {
        Position  = position;
        Intensity = intensity;
    }

    public static Light Create(Fewple position, Color intensity) =>
        new(position, intensity);
}
