using RayGround.Core.Models;

namespace RayGround.Core;

public class Canvas
{
    public readonly float Width;
    public readonly float Height;

    public readonly List<Pixel> Pixels = [];

    public Canvas(float width, float height, Color? defaultColor = null)
    {
        Width  = width;
        Height = height;
        for (var i = 0; i <= Width; i++) // Rows
        for (var j = 0; j <= Height; j++)  // Columns
            Pixels.Add(new Pixel(Fewple.NewPoint(i, j, 0), defaultColor));
    }
}
