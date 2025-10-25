using RayGround.Cli.Rendering.Blueprinting;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Models;

namespace RayGround.Cli.Rendering.Capturing;

public class CaptureEnclosure(Camera camera, World world, int maxDepth = 14) : ICapturable
{
    public async Task<Capture> CaptureAsync(IConstructable blueprint)
    {
        var stamped = await blueprint.ConstructAsync(world);
        var render  = camera.Render(stamped.Imprint, maxDepth);

        return Capture.Create(stamped.Name, render);
    }
}
