namespace RayGround.Core;

public class RayPixel(RayTuple position, RayColor? color)
{
    public readonly RayTuple Position = position;
    public readonly RayColor? Color   = color ?? RayColor.Create(0,0,0);
}
