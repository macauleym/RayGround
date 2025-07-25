using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Operations;

namespace RayGround.Tests;

public class MaterialTests
{
    [Fact]
    public void MaterialHasSaneDefaultValues()
    {
        // Arrange
        
        // Act
        var actual = Material.Create();

        // Assert
        actual.Ambient.Should().Be(0.1f);
        actual.Diffuse.Should().Be(0.9f);
        actual.Specular.Should().Be(0.9f);
        actual.Shininess.Should().Be(200.0f);
    }

    [Fact]
    public void MaterialIsLitWhenEyeAlongLight()
    {
        // Arrange
        var material  = Material.Create();
        var position  = RayTuple.NewPoint(0, 0, 0);
        var eyeVec    = RayTuple.NewVector(0, 0, -1);
        var normalVec = RayTuple.NewVector(0, 0, -1);
        var light     = Light.AsPoint(RayTuple.NewPoint(0, 0, -10), RayColor.Create(1, 1, 1));
        var expected  = RayColor.Create(1.9f, 1.9f, 1.9f);

        // Act
        var actual = Illuminate.Lighting(material, light, position, eyeVec, normalVec);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void MaterialIsLitWhenEyeOffset45Degrees()
    {
        // Arrange
        var material  = Material.Create();
        var position  = RayTuple.NewPoint(0, 0, 0);
        var eyeVec    = RayTuple.NewVector(0, MathF.Sqrt(2)/2, -MathF.Sqrt(2)/2);
        var normalVec = RayTuple.NewVector(0, 0, -1);
        var light     = Light.AsPoint(RayTuple.NewPoint(0, 0, -10), RayColor.Create(1, 1, 1));
        var expected  = RayColor.Create(1.0f, 1.0f, 1.0f);

        // Act
        var actual = Illuminate.Lighting(material, light, position, eyeVec, normalVec);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void MaterialIsLitWhenLightOffset45Degrees()
    {
        // Arrange
        var material  = Material.Create();
        var position  = RayTuple.NewPoint(0, 0, 0);
        var eyeVec    = RayTuple.NewVector(0, 0, -1);
        var normalVec = RayTuple.NewVector(0, 0, -1);
        var light     = Light.AsPoint(RayTuple.NewPoint(0, 10, -10), RayColor.Create(1, 1, 1));
        var expected  = RayColor.Create(0.7364f, 0.7364f, 0.7364f);

        // Act
        var actual = Illuminate.Lighting(material, light, position, eyeVec, normalVec);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void MaterialIsLitWhenEyeInReflectionPath()
    {
        // Arrange
        var material  = Material.Create();
        var position  = RayTuple.NewPoint(0, 0, 0);
        var eyeVec    = RayTuple.NewVector(0, -MathF.Sqrt(2)/2, -MathF.Sqrt(2)/2);
        var normalVec = RayTuple.NewVector(0, 0, -1);
        var light     = Light.AsPoint(RayTuple.NewPoint(0, 10, -10), RayColor.Create(1, 1, 1));
        var expected  = RayColor.Create(1.6363853f, 1.6363853f, 1.6363853f);

        // Act
        var actual = Illuminate.Lighting(material, light, position, eyeVec, normalVec);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void MaterialIsNotLitWhenLightBehindSurface()
    {
        // Arrange
        var material  = Material.Create();
        var position  = RayTuple.NewPoint(0, 0, 0);
        var eyeVec    = RayTuple.NewVector(0, 0, -1);
        var normalVec = RayTuple.NewVector(0, 0, -1);
        var light     = Light.AsPoint(RayTuple.NewPoint(0, 0, 10), RayColor.Create(1, 1, 1));
        var expected  = RayColor.Create(0.1f, 0.1f, 0.1f);

        // Act
        var actual = Illuminate.Lighting(material, light, position, eyeVec, normalVec);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
