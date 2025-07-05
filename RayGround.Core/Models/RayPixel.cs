namespace RayGround.Core;

public struct RayPixel(RayTuple position, RayColor color)
{
    public readonly RayTuple Position = position;
    public readonly RayColor Color    = color;
}
