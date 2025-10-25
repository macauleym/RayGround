using RayGround.Cli.Rendering.Blueprinting;

namespace RayGround.Cli.Rendering.Capturing;

public interface ICapturable
{
    Task<Capture> CaptureAsync(IConstructable blueprint);
}
