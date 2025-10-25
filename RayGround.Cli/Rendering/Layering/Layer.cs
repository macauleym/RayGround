using RayGround.Cli.Rendering.Capturing;

namespace RayGround.Cli.Rendering.Layering;

public abstract class Layer : ILayerable
{
    public abstract Task<Capture> ApplyAsync(Capture source);
}
