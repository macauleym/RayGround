using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Models;

namespace RayGround.Cli.Rendering;

public class CanvasBrush : IStrokable
{
    public void Stroke(Canvas canvas, Fewple position, Color? color)
    {
        var x = position.X;
        if (x > canvas.Width)
            x = canvas.Width;
        if (x < 0)
            x = 0;

        var y = position.Y;
        if (y > canvas.Height)
            y = canvas.Height;
        if (y < 0)
            y = 0;
    
        canvas.WritePixel((int)x, (int)canvas.Height - (int)y, color);
    }
}
