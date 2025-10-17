namespace RayGround.Core.Models;

public class Camera
{
    public readonly float HorizontalSize;
    public readonly float VerticalSize;
    public readonly float FieldOfView;
    
    public readonly Matrix Transform;

    public readonly CameraCanvas Canvas;

    Camera(
      float horizontalSize
    , float verticalSize
    , float fieldOfView
    , Matrix? transform
    , CameraCanvas canvas
    ) {
        HorizontalSize = horizontalSize;
        VerticalSize   = verticalSize;
        FieldOfView    = fieldOfView;
        
        Transform = transform ?? Matrix.Identity;

        Canvas = canvas;
    }

    public static Camera Create(float h, float v, float f, Matrix? t = null) =>
        new(h, v, f, t, CameraCanvas.Create(h, v, f));
}
