using RayGround.Cli.Logging;
using RayGround.Cli.Rendering.Blueprinting.Blueprints;

namespace RayGround.Cli.Rendering.Blueprinting;

public class BlueprintFactory(ILogable logger)
{
    public IConstructable Construct(Render desired) =>
        desired switch
        { Render.Projectile   => new Projectile(logger)
        , Render.Matrix       => new MatrixManipulation(logger)
        , Render.Clock        => new Clock(logger)
        , Render.Camera       => new CameraView()
        , Render.Shadows      => new SimpleShadows()
        , Render.Plane        => new Plane()
        , Render.Patterns     => new Pattern()
        , Render.Reflectfrac  => new ReflectionRefraction()
        , Render.GuysWithCube => new GuysWithTheCube()
        , _                   => new BlankBlueprint()
        };
}
