namespace RayGround.Core.Models;

public class CameraCanvas
{
    public readonly float HalfWidth;
    public readonly float HalfHeight;
    public readonly float PixelSize;

    CameraCanvas(float halfWidth, float halfHeight, float pixelSize)
    {
        HalfHeight = halfHeight;
        HalfWidth  = halfWidth;
        PixelSize  = pixelSize;
    }

    public static CameraCanvas Create(float camHorizontal, float camVertical, float camFov)
    {
        var halfView = MathF.Tan(camFov / 2);
        var aspect   = camHorizontal / camVertical;

        var aspectAtLeastOne = aspect >= 1;
        var halfWidth        = aspectAtLeastOne ? halfView          : halfView * aspect;
        var halfHeight       = aspectAtLeastOne ? halfView / aspect : halfView;
        var pixelSize        = (halfWidth * 2) / camHorizontal;
        
        return new CameraCanvas(
          halfWidth
        , halfHeight
        , pixelSize
        );
    }
}
