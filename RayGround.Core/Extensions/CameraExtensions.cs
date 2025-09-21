using RayGround.Core.Calculators;
using RayGround.Core.Models;

namespace RayGround.Core.Extensions;

public static class CameraExtensions
{
    public static Camera Morph(this Camera source, RayMatrix transform) =>
        CameraCalculator.Morph(source, transform);

    public static Ray RayForPixel(this Camera source, float pixelX, float pixelY) =>
        CameraCalculator.RayForPixel(source, pixelX, pixelY);

    public static RayCanvas Render(this Camera source, World toRender) =>
        CameraCalculator.Render(source, toRender);
}
