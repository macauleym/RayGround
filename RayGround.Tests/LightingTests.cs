using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Models;

namespace RayGround.Tests;

public class LightingTests
{
    [Fact]
    public void APointLightHasPositionAndIntensity()
    {
        // Arrange
        var position  = Fewple.NewPoint(0, 0, 0);
        var intensity = Color.Create(1, 1, 1);
        
        // Act
        var pointLight = Light.Create(position, intensity);
        
        // Assert
        pointLight.Position.Should().Be(position);
        pointLight.Intensity.Should().Be(intensity);
    }
}
