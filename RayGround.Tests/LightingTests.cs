using FluentAssertions;
using RayGround.Core;

namespace RayGround.Tests;

public class LightingTests
{
    [Fact]
    public void APointLightHasPositionAndIntensity()
    {
        // Arrange
        var position  = RayTuple.NewPoint(0, 0, 0);
        var intensity = RayColor.Create(1, 1, 1);
        
        // Act
        var pointLight = Light.Create(position, intensity);
        
        // Assert
        pointLight.Position.Should().Be(position);
        pointLight.Intensity.Should().Be(intensity);
    }
}
