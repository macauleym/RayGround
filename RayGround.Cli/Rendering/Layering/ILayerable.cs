using RayGround.Cli.Rendering.Capturing;

namespace RayGround.Cli.Rendering.Layering;

public interface ILayerable
{
    Task<Capture> ApplyAsync(Capture source);
}
