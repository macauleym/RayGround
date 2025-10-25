using RayGround.Cli.Rendering.Capturing;

namespace RayGround.Cli.Rendering.Layering;

public class EmptyLayer : Layer
{
    public override Task<Capture> ApplyAsync(Capture source) => Task.FromResult(source);
}
