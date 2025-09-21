using RayGround.Core.Extensions;
using RayGround.Core.Models;

namespace RayGround.Core.Calculators;

public class CameraCalculator
{
    public static Camera Morph(Camera source, RayMatrix transform) => 
        Camera.Create(source.HorizontalSize, source.VerticalSize, source.FieldOfView, transform);

    public static Ray RayForPixel(Camera source, float pixelX, float pixelY)
    {
        // The offset from the edge of the canvas, to the center of the pixel.
        var xOffset = (pixelX + 0.5f) * source.Canvas.PixelSize;
        var yOffset = (pixelY + 0.5f) * source.Canvas.PixelSize;
        
        // The untransformed pixel's world coordinates.
        var worldX = source.Canvas.HalfWidth - xOffset;
        var worldY = source.Canvas.HalfHeight - yOffset;
        
        // Transform the canvas point and origin using the camera's
        // transform matrix, then get the ray's direction.
        var inverseCamera = source.Transform.Inverse();
        var pixel = inverseCamera * RayTuple.NewPoint(worldX, worldY, -1);
        var origin = inverseCamera * RayTuple.NewPoint(0, 0, 0);
        var direction = (pixel - origin).ToTuple().Normalize();
        
        return Ray.Create(origin.ToTuple(), direction);
    }

    static Task<List<RayPixel>> RenderPixelsChunkAsync(Camera cam, World world, RenderChunk chunk) => Task.Run(() =>
    {
        var chunkPixels = new List<RayPixel>();
        for (var y = chunk.VerticalMin; y <= chunk.VerticalMax; y++)
        for (var x = chunk.HorizontalMin; x <= chunk.HorizontalMax; x++)
        {
            var ray   = cam.RayForPixel(x, y);
            var color = world.ColorAt(ray);
            chunkPixels.Add(new RayPixel(RayTuple.NewPoint(x, y, 0), color));
        }

        return chunkPixels;
    });
    
    public static async Task<RayCanvas> RenderAsync(Camera source, World toRender)
    {
        var canvas = new RayCanvas(source.HorizontalSize, source.VerticalSize);
        
        var chunkCount    = 4;
        var vertChunkSize = float.Round(source.VerticalSize   / chunkCount, MidpointRounding.ToZero);
        var hztlChunkSize = float.Round(source.HorizontalSize / chunkCount, MidpointRounding.ToZero);
        var chunks = Enumerable.Range(0, chunkCount - 1)
            .Select(i =>
            {
                var vertOffset = vertChunkSize * i;
                var hztlOffset = hztlChunkSize * i;
                
                return RenderChunk.Create(
                  0 + vertOffset
                , vertChunkSize - 1 + vertOffset
                , 0 + hztlOffset
                , hztlChunkSize - 1 + hztlOffset
                );
            })
            .ToList();
        chunks.Add(RenderChunk.Create(
          chunks.Last().VerticalMin+vertChunkSize
        , source.VerticalSize - 1
        , chunks.Last().HorizontalMin+hztlChunkSize
        , source.HorizontalSize - 1
        ));

        var chunkTasks = chunks
            .Select(chunkInfo => RenderPixelsChunkAsync(source, toRender, chunkInfo))
            .ToArray();
        await Task.WhenAll(chunkTasks);

        var pixels = chunkTasks.SelectMany(t => t.Result);
        canvas.Pixels.AddRange(pixels);

        return canvas;
    }

    public static RayCanvas Render(Camera source, World toRender)
    {
        var canvas = new RayCanvas(source.HorizontalSize, source.VerticalSize);

        for (var y = 0; y <= source.VerticalSize - 1; y++)
        for (var x = 0; x <= source.HorizontalSize - 1; x++)
        {
            var ray   = source.RayForPixel(x, y);
            var color = toRender.ColorAt(ray);
            canvas.WritePixel(x, y, color);
        }

        return canvas;
    }
}
