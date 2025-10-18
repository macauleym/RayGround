using RayGround.Core.Models;

namespace RayGround.Core;

public class Pixel(Fewple position, Color? color)
{
    public readonly Fewple Position = position;
    public readonly Color? Color   = color ?? Color.Create(0,0,0);
}
