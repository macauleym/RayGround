using RayGround.Cli.Logging;
using RayGround.Cli.Rendering;
using RayGround.Core.Interfaces;

namespace RayGround.Cli.Building;

public class RendererFactory(IStrokable brush, IExportCanvas exporter, ILogable logger)
{
    public IRenderable Construct(Render desired) =>
        desired switch
        {
            Render.Projectile =>
                // Chapter 2
                new ProjectileRenderer(brush, exporter, logger),
            Render.Matrix =>
                // Chapter 3
                new MatrixManipulationRenderer(exporter, logger),
            Render.Clock =>
                // Chapter 4
                new ClockRenderer(brush, exporter, logger),
            Render.Sphere =>
                // Chapter 5
                new SphereRenderer(exporter, logger),
            Render.Shaded =>
                // Chapter 6
                new ShadedRenderer(exporter, logger),
            Render.Camera =>
                // Chapter 7
                new CameraViewRenderer(exporter, logger),
            Render.Shadows =>
                // Chapter 8
                new SimpleShadowsRenderer(exporter, logger),
            Render.Plane =>
                // Chapter 9
                new PlanesRenderer(exporter, logger),
            Render.Patterns =>
                // Chapter 10
                new PatternsRenderer(exporter, logger),
            _ => 
                throw new ArgumentOutOfRangeException(nameof(desired), desired, null)
        };
}
