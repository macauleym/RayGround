using RayGround.Core.Models;

namespace RayGround.Core.Extensions;

public static class RayCanvasExtensions
{
    public static void WritePixel(this Canvas source, int x, int y, Color? color)
    {
        var toWrite     = new Pixel(Fewple.NewPoint(x, y, 0), color);
        var targetIndex = source.Pixels.FindIndex(p =>
            MathF.Abs(p.Position.X - x) < Fewple.EPSILON
            && MathF.Abs(p.Position.Y - y) < Fewple.EPSILON
            );
        try
        {
            if (targetIndex < 0)
                return;
            
            source.Pixels[targetIndex] = toWrite;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static Pixel? GetPixel(this Canvas source, int x, int y) =>
        source.Pixels.Find(p => 
            MathF.Abs(p.Position.X - x) < Fewple.EPSILON 
            && MathF.Abs(p.Position.Y - y) < Fewple.EPSILON
            );
}
