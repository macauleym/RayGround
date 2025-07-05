namespace RayGround.Core;

public class Environment(RayTuple gravity, RayTuple wind)
{
    public readonly RayTuple Gravity = gravity;
    public readonly RayTuple Wind = wind;
}
