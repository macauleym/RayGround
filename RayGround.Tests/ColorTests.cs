using FluentAssertions;
using RayGround.Core;

namespace RayGround.Tests;

public class ColorTests
{
    [Fact]
    public void ColorsAreTuples()
    {
        // Arrange
        var red   = -0.5f;
        var green = 0.4f;
        var blue  = 1.7f;
        
        // Act
        var actual = Color.Create(red, green, blue);
        
        // Assert
        actual.Red.Should().Be(red);
        actual.Green.Should().Be(green);
        actual.Blue.Should().Be(blue);
    }

    [Fact]
    public void AddingColorsReturnsCombinedColor()
    {
        // Arrange
        var a        = Color.Create(0.9f, 0.6f, 0.75f);
        var b        = Color.Create(0.7f, 0.1f, 0.25f);
        var expected = Color.Create(1.6f, 0.7f, 1.0f);

        // Act
        var actual = a + b;

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void SubtractingColorsReturnsReducedColor()
    {
        // Arrange
        var a        = Color.Create(0.9f, 0.6f, 0.75f);
        var b        = Color.Create(0.7f, 0.1f, 0.25f);
        var expected = Color.Create(0.2f, 0.5f, 0.5f);
        
        // Act
        var actual = a - b;
        
        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void MultiplyingColorByScalerReturnsAmplifiedColor()
    {
        // Arrange
        var color    = Color.Create(0.2f, 0.3f, 0.4f);
        var scaler   = 2;
        var expected = Color.Create(0.4f, 0.6f, 0.8f);
        
        // Act
        var actual = color * scaler;
        
        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void MultiplyingColorsTogetherReturnsBlendedColor()
    {
        // Arrange
        var a        = Color.Create(1, 0.2f, 0.4f);
        var b        = Color.Create(0.9f, 1f, 0.1f);
        var expected = Color.Create(0.9f, 0.2f, 0.04f);
        
        // Act
        var actual = a * b;
        
        // Assert
        actual.Should().Be(expected);
    }
}
