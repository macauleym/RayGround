using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Operations;

namespace RayGround.Tests;

public class CameraTests
{
    [Fact]
    public void ConfirmDefaultCamera()
    {
        // Arrange
        var hSize = 160;
        var vSize = 120;
        var fov   = float.Pi / 2;

        // Act
        var actual = Camera.Create(hSize, vSize, fov);

        // Assert
        actual.HorizontalSize.Should().Be(hSize);
        actual.VerticalSize.Should().Be(vSize);
        actual.FieldOfView.Should().Be(fov);
        actual.Transform.Should().BeEquivalentTo(Matrix.Identity);
    }

    [Fact]
    public void ConfirmHorizontalPixelCanvas()
    {
        // Arrange
        var expected = 0.01f;

        // Act
        var actual = Camera.Create(200, 125, float.Pi / 2);

        // Assert
        actual.Canvas.PixelSize.Should().Be(expected);
    }
    
    [Fact]
    public void ConfirmVerticalPixelCanvas()
    {
        // Arrange
        var expected = 0.01f;

        // Act
        var actual = Camera.Create(125, 200, float.Pi / 2);

        // Assert
        actual.Canvas.PixelSize.Should().Be(expected);
    }

    [Fact]
    public void ConstructARayPassingThroughCanvasCenter()
    {
        // Arrange
        var cam      = Camera.Create(201, 101, float.Pi / 2);
        var expected = Ray.Create(
          Fewple.NewPoint(0, 0, 0)
        , Fewple.NewVector(0, 0, -1)
        );

        // Act
        var actual = cam.RayForPixel(100, 50);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ConstructARayPassingThroughCornerOfCanvas()
    {
        // Arrange
        var cam      = Camera.Create(201, 101, float.Pi / 2);
        var expected = Ray.Create(Fewple.NewPoint(0, 0, 0), Fewple.NewVector(0.66519f, 0.33259f, -0.66851f));

        // Act
        var actual = cam.RayForPixel(0, 0);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ConstructARayAfterCameraTransformation()
    {
        // Arrange
        var cam = Camera
            .Create(201, 101, float.Pi / 2)
            .Morph(Transform.RotationY(float.Pi / 4) * Transform.Translation(0, -2, 5));
        var expected = Ray.Create(
          Fewple.NewPoint(0, 2, -5)
        , Fewple.NewVector(float.Sqrt(2)/2f, 0f, -float.Sqrt(2)/2f)
        );

        // Act
        var actual = cam.RayForPixel(100, 50);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CanRenderAGivenWorld()
    {
        // Arrange
        var world  = World.Default();
        var from   = Fewple.NewPoint(0, 0, -5);
        var to     = Fewple.NewPoint(0, 0, 0);
        var up     = Fewple.NewVector(0, 1, 0);
        var camera = Camera.Create(11, 11, float.Pi / 2)
            .Morph(View.Transform(from, to, up));
        var expected = Color.Create(0.38066f, 0.47583f, 0.2855f);
        
        // Act
        Canvas actual = camera.Render(world);

        // Assert
        actual.GetPixel(5, 5).Color.Should().BeEquivalentTo(expected);
    }
}
