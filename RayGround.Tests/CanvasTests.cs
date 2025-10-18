using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Exporters;
using RayGround.Core.Extensions;
using RayGround.Core.Models;

namespace RayGround.Tests;

public class CanvasTests
{
    [Fact]
    public void CreateANewCanvas()
    {
        // Arrange
        var width  = 10;
        var height = 20;
        var black  = Color.Create(0,0,0);
        
        // Act
        var actual = new Canvas(width, height);
        
        // Assert
        actual.Width.Should().Be(width);
        actual.Height.Should().Be(height);
        actual.Pixels.Should().AllSatisfy(p => p
            .Color.Should().Be(black));
    }

    [Fact]
    public void WritePixelUpdatesExistingColor()
    {
        // Arrange
        var color    = Color.Create(1, 0, 0);
        var x        = 2;
        var y        = 3;
        var expected = new Pixel(Fewple.NewPoint(x, y, 0), color);
        var target   = new Canvas(10, 20);
        
        // Act
        target.WritePixel(x, y, color);
        
        // Assert
        target.GetPixel(x, y).Should().BeEquivalentTo(expected);
    }
}
