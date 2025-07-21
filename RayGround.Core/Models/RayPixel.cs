namespace RayGround.Core;

public class RayPixel(RayTuple position, RayColor? color)
{
    public readonly RayTuple Position = position;
    public readonly RayColor? Color   = color ?? new RayColor(0,0,0);
}
