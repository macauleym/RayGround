namespace RayGround.Core.Extensions;

public static class RayCanvasExtensions
{
    public static void WritePixel(this RayCanvas source, int x, int y, RayColor? color)
    {
        var toWrite     = new RayPixel(RayTuple.NewPoint(x, y, 0), color);
        var targetIndex = source.Pixels.FindIndex(p =>
            MathF.Abs(p.Position.X - x) < RayTuple.EPSILON
            && MathF.Abs(p.Position.Y - y) < RayTuple.EPSILON
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

    public static RayPixel? GetPixel(this RayCanvas source, int x, int y) =>
        source.Pixels.Find(p => 
            MathF.Abs(p.Position.X - x) < RayTuple.EPSILON 
            && MathF.Abs(p.Position.Y - y) < RayTuple.EPSILON
            );
}
