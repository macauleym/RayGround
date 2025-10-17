using RayGround.Core.Models;

namespace RayGround.Core;

public class Environment(Fewple gravity, Fewple wind)
{
    public readonly Fewple Gravity = gravity;
    public readonly Fewple Wind    = wind;
}
