using RayGround.Core.Models;

namespace RayGround.Core;

public class RayPixel(Fewple position, RayColor? color)
{
    public readonly Fewple Position = position;
    public readonly RayColor? Color   = color ?? RayColor.Create(0,0,0);
}
