using RayGround.Cli.Logging;

namespace RayGround.Cli.Rendering.Layering;

public class LayerFactory(ILogable logger)
{
    public ILayerable Construct(Render desired) =>
        desired switch
        { Render.Sphere => new ScratchSphere()
        , Render.Shaded => new ShadedSphere(logger)
        , _             => new EmptyLayer()
        };
}
