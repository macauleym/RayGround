namespace RayGround.Core.Models;

public class RenderChunk
{
    public readonly float VerticalMin;
    public readonly float VerticalMax;
    public readonly float HorizontalMin;
    public readonly float HorizontalMax;

    RenderChunk(float vMin, float vMax, float hMin, float hMax)
    {
        VerticalMin   = vMin;
        VerticalMax   = vMax;
        HorizontalMin = hMin;
        HorizontalMax = hMax;
    }

    public static RenderChunk Create(float vMin, float vMax, float hMin, float hMax) =>
        new(vMin, vMax, hMin, hMax);
}
