using RayGround.Cli.Rendering.Capturing;

namespace RayGround.Cli.Rendering.Printing;

public interface IDevelopable
{
    Task<string> DevelopAsync(Capture capture);
}
