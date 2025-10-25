using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Models;
using RayGround.Core.Models.Patterns;
using RayGround.Core.Operations;

namespace RayGround.Tests;

public class MaterialTests
{
    readonly Sphere dummyEntity = Sphere.Create();
    
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
        var position  = Fewple.NewPoint(0, 0, 0);
        var eyeVec    = Fewple.NewVector(0, 0, -1);
        var normalVec = Fewple.NewVector(0, 0, -1);
        var light     = Light.Create(Fewple.NewPoint(0, 0, -10), Color.Create(1, 1, 1));
        var expected  = Color.Create(1.9f, 1.9f, 1.9f);

        // Act
        var actual = Illuminate.Lighting(material, dummyEntity, light, position, eyeVec, normalVec, false);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void MaterialIsLitWhenEyeOffset45Degrees()
    {
        // Arrange
        var material  = Material.Create();
        var position  = Fewple.NewPoint(0, 0, 0);
        var eyeVec    = Fewple.NewVector(0, MathF.Sqrt(2)/2, -MathF.Sqrt(2)/2);
        var normalVec = Fewple.NewVector(0, 0, -1);
        var light     = Light.Create(Fewple.NewPoint(0, 0, -10), Color.Create(1, 1, 1));
        var expected  = Color.Create(1.0f, 1.0f, 1.0f);

        // Act
        var actual = Illuminate.Lighting(material, dummyEntity, light, position, eyeVec, normalVec, false);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void MaterialIsLitWhenLightOffset45Degrees()
    {
        // Arrange
        var material  = Material.Create();
        var position  = Fewple.NewPoint(0, 0, 0);
        var eyeVec    = Fewple.NewVector(0, 0, -1);
        var normalVec = Fewple.NewVector(0, 0, -1);
        var light     = Light.Create(Fewple.NewPoint(0, 10, -10), Color.Create(1, 1, 1));
        var expected  = Color.Create(0.7364f, 0.7364f, 0.7364f);

        // Act
        var actual = Illuminate.Lighting(material, dummyEntity, light, position, eyeVec, normalVec, false);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void MaterialIsLitWhenEyeInReflectionPath()
    {
        // Arrange
        var material  = Material.Create();
        var position  = Fewple.NewPoint(0, 0, 0);
        var eyeVec    = Fewple.NewVector(0, -MathF.Sqrt(2)/2, -MathF.Sqrt(2)/2);
        var normalVec = Fewple.NewVector(0, 0, -1);
        var light     = Light.Create(Fewple.NewPoint(0, 10, -10), Color.Create(1, 1, 1));
        var expected  = Color.Create(1.6363853f, 1.6363853f, 1.6363853f);

        // Act
        var actual = Illuminate.Lighting(material, dummyEntity, light, position, eyeVec, normalVec, false);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void MaterialIsNotLitWhenLightBehindSurface()
    {
        // Arrange
        var material  = Material.Create();
        var position  = Fewple.NewPoint(0, 0, 0);
        var eyeVec    = Fewple.NewVector(0, 0, -1);
        var normalVec = Fewple.NewVector(0, 0, -1);
        var light     = Light.Create(Fewple.NewPoint(0, 0, 10), Color.Create(1, 1, 1));
        var expected  = Color.Create(0.1f, 0.1f, 0.1f);

        // Act
        var actual = Illuminate.Lighting(material, dummyEntity, light, position, eyeVec, normalVec, false);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void MaterialSurfaceHasShadowWhenObscured()
    {
        // Arrange
        var material = Material.Create();
        var position = Fewple.NewPoint(0, 0, 0);
        var eye      = Fewple.NewVector(0, 0, -1);
        var normal   = Fewple.NewVector(0, 0, -1);
        var light    = Light.Create(Fewple.NewPoint(0, 0, -10), Color.Create(1, 1, 1));
        var inShadow = true;
        var expected = Color.Create(0.1f, 0.1f, 0.1f);

        // Act
        var actual = Illuminate.Lighting(material, dummyEntity, light, position, eye, normal, inShadow);

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void MaterialCanHavePatternApplied()
    {
        // Arrange
        var stripe   = Stripe.Create(Color.White, Color.Black);
        var material = Material
            .Create(
              ambient: 1
            , diffuse: 0
            , specular: 0)
            .Etch(stripe);
        
        var eye    = Fewple.NewVector(0, 0, -1);
        var normal = Fewple.NewVector(0, 0, -1);
        var light  = Light.Create(Fewple.NewPoint(0, 0, -10), Color.White);
        
        // Act
        var actual = new[] { 0.9f, 1.1f }
            .Select(f => Illuminate.Lighting(material, dummyEntity, light, Fewple.NewPoint(f, 0, 0), eye, normal, false))
            .ToArray();
        
        // Assert
        actual[0].Should().BeEquivalentTo(Color.White);
        actual[1].Should().BeEquivalentTo(Color.Black);
    }

    [Fact]
    public void MaterialAllowsReflection()
    {
        // Arrange
        
        // Act
        var actual = Material.Create();

        // Assert
        actual.Reflective.Should().Be(0.0f);
    }

    [Fact]
    public void MaterialHasDefaultTransparencyAndRefraction()
    {
        // Arrange
        
        // Act
        var actual = Material.Create();

        // Assert
        actual.Transparency.Should().Be(0f);
        actual.RefractionIndex.Should().Be(1f);
    }
}
