namespace RayGround.Core;

public class RayCanvas
{
    public readonly float Width;
    public readonly float Height;

    public readonly List<RayPixel> Pixels = new();

    public RayCanvas(float width, float height, RayColor? defaultColor = null)
    {
        Width  = width;
        Height = height;
        for (var i = 0; i <= Width; i++) // Rows
        for (var j = 0; j <= Height; j++)  // Columns
            Pixels.Add(new RayPixel(RayTuple.NewPoint(i, j, 0), defaultColor));
    }
}
