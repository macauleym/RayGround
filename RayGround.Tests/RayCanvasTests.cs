using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Exporters;
using RayGround.Core.Extensions;

namespace RayGround.Tests;

public class RayCanvasTests
{
    PPMExporter ppm = new();
    
    [Fact]
    public void CreateANewCanvas()
    {
        // Arrange
        var width  = 10;
        var height = 20;
        var black  = new RayColor();
        
        // Act
        var actual = new RayCanvas(width, height);
        
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
        var color    = new RayColor(1, 0, 0);
        var x        = 2;
        var y        = 3;
        var expected = new RayPixel(RayTuple.NewPoint(x, y, 0), color);
        var target   = new RayCanvas(10, 20);
        
        // Act
        target.WritePixel(x, y, color);
        
        // Assert
        target.GetPixel(x, y).Should().Be(expected);
    }

    [Fact]
    public async Task ExportsCorrectPPMHeader()
    {
        // Arrange
        var target = new RayCanvas(5, 3);
        var expected =
"""
P3
5 3
255
""";

        // Act
        var actual = await ppm.ExportAsync(target);
        
        // Assert
        actual.Should().Contain(expected);
    }

    [Fact]
    public async Task ExportsCorrectPixelData()
    {
        // Arrange
        var color1 = new RayColor(1.5f, 0, 0);
        var color2 = new RayColor(0, 0.5f, 0);
        var color3 = new RayColor(-0.5f, 0, 1);
        
        var target = new RayCanvas(5, 3);
        target.WritePixel(0, 0, color1);
        target.WritePixel(2, 1, color2);
        target.WritePixel(4, 2, color3);

        var expected =
"""
255 0 0 0 0 0 0 0 0 0 0 0 0 0 0
0 0 0 0 0 0 0 128 0 0 0 0 0 0 0
0 0 0 0 0 0 0 0 0 0 0 0 0 0 255
""";

        // Act
        var actual = await ppm.ExportAsync(target);
        
        // Assert
        actual.Should().Contain(expected);
    }

    [Fact]
    public async Task ExportsWrappedPixelDataLines()
    {
        // Arrange
        var target = new RayCanvas(10, 2, new RayColor(1f, 0.8f, 0.6f));
        var expected = 
"""
255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204
153 255 204 153 255 204 153 255 204 153 255 204 153
255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204
153 255 204 153 255 204 153 255 204 153 255 204 153
""";

        // Act
        var actual = await ppm.ExportAsync(target);

        // Assert
        actual.Should().Contain(expected);
    }

    [Fact]
    public async Task ExportEndsWithNewline()
    {
        // Arrange
        var target = new RayCanvas(3, 4);
        var expected = "\n";

        // Act
        var actual = await ppm.ExportAsync(target);

        // Assert
        actual.Should().EndWith(expected);
    }
}
