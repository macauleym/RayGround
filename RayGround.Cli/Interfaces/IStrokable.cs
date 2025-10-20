using RayGround.Core;
using RayGround.Core.Models;

namespace RayGround.Cli.Rendering;

public interface IStrokable
{
    void Stroke(Canvas canvas, Fewple position, Color? color);
}
