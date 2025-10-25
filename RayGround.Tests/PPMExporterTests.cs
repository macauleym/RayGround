using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Exporters;
using RayGround.Core.Extensions;

namespace RayGround.Tests;

public class PPMExporterTests
{
    PPMExporter testExporter = new();
    
    [Fact]
    public async Task ExportsCorrectHeader()
    {
        // Arrange
        var canvas   = new Canvas(5, 3);
        var expected =
"""
P3
5 3
255
""";

        // Act
        var actual = await testExporter.EncodeString(canvas);
        
        // Assert
        actual.Should().Contain(expected);
    }

    [Fact]
    public async Task ExportsCorrectPixelData()
    {
        // Arrange
        var color1 = Color.Create(1.5f, 0, 0);
        var color2 = Color.Create(0, 0.5f, 0);
        var color3 = Color.Create(-0.5f, 0, 1);
        
        var canvas = new Canvas(5, 3);
        canvas.WritePixel(0, 0, color1);
        canvas.WritePixel(2, 1, color2);
        canvas.WritePixel(4, 2, color3);

        var expected =
"""
255 0 0 0 0 0 0 0 0 0 0 0 0 0 0
0 0 0 0 0 0 0 128 0 0 0 0 0 0 0
0 0 0 0 0 0 0 0 0 0 0 0 0 0 255
""";

        // Act
        var actual = await testExporter.EncodeString(canvas);
        
        // Assert
        actual.Should().Contain(expected);
    }

    [Fact]
    public async Task ExportsWrappedPixelDataLines()
    {
        // Arrange
        var canvas   = new Canvas(10, 2, Color.Create(1f, 0.8f, 0.6f));
        var expected = 
"""
255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204
153 255 204 153 255 204 153 255 204 153 255 204 153
255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204
153 255 204 153 255 204 153 255 204 153 255 204 153
""";

        // Act
        var actual = await testExporter.EncodeString(canvas);

        // Assert
        actual.Should().Contain(expected);
    }

    [Fact]
    public async Task ExportEndsWithNewline()
    {
        // Arrange
        var canvas   = new Canvas(3, 4);
        var expected = "\n";

        // Act
        var actual = await testExporter.EncodeString(canvas);

        // Assert
        actual.Should().EndWith(expected);
    }
}
