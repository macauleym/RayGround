using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Models.Entities;
using RayGround.Core.Operations;

namespace RayGround.Tests;

public class SphereTests
{
    [Fact]
    public void DefaultSphereTransformIsIdentity()
    {
        // Arrange
        var sphere   = Sphere.Create();
        var expected = Matrix.Identity;

        // Act
        var actual = sphere.Transform;

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void PerformOperationOnSphereTransform()
    {
        // Arrange
        var sphere    = Sphere.Create();
        var translate = Transform.Translation(2, 3, 4);
        
        // Act
        var actual = sphere.Morph(translate);
        
        // Assert
        actual.Transform.Should().BeEquivalentTo(translate);
    }

    [Fact]
    public void IntersectAScaledSphereWithRay()
    {
        // Arrange
        var ray    = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
        var sphere = Sphere.Create();
        var scale  = Transform.Scaling(2, 2, 2);
        sphere     = sphere.Morph(scale).As<Sphere>();
        List<Intersection> expected = [
              Intersection.Create(3, sphere)
            , Intersection.Create(7, sphere)
            ];
        
        // Act
        var actual = ray.Intersect(sphere);
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void IntersectATranslatedSphereWithRay()
    {
        var ray    = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
        var scale  = Transform.Translation(5, 0, 0);
        var sphere = Sphere.Create().Morph(scale).As<Sphere>();
        
        // Act
        var actual = sphere.Intersections(sphere.BindRay(ray));
        
        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void CorrectNormalOnSphereOnXAxis()
    {
        // Arrange
        var sphere   = Sphere.Create();
        var expected = Fewple.NewVector(1, 0, 0);

        // Act
        var actual = sphere.NormalAt(Fewple.NewPoint(1, 0, 0));

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CorrectNormalOnSphereOnYAxis()
    {
        // Arrange
        var sphere   = Sphere.Create();
        var expected = Fewple.NewVector(0, 1, 0);
        
        // Act
        var actual = sphere.NormalAt(Fewple.NewVector(0, 1, 0));
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CorrectNormalOnSphereOnZAxis()
    {
        // Arrange
        var sphere   = Sphere.Create();
        var expected = Fewple.NewVector(0, 0, 1);
        
        // Act
        var actual = sphere.NormalAt(Fewple.NewVector(0, 0, 1));
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CorrectNormalOnSphereAtNonAxial()
    {
        // Arrange
        var sphere   = Sphere.Create();
        var expected = Fewple.NewVector(MathF.Sqrt(3)/3, MathF.Sqrt(3)/3, MathF.Sqrt(3)/3);
        
        // Act
        var actual = sphere.NormalAt(Fewple.NewVector(MathF.Sqrt(3)/3, MathF.Sqrt(3)/3, MathF.Sqrt(3)/3));
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void NormalIsANormalizedVector()
    {
        // Arrange
        var sphere = Sphere.Create();
        
        // Act
        var actual = sphere.NormalAt(Fewple.NewVector(MathF.Sqrt(3)/3, MathF.Sqrt(3)/3, MathF.Sqrt(3)/3));
        
        // Assert
        actual.Should().BeEquivalentTo(actual.Normalize());
    }
    
    [Fact]
    public void SphereIsAnEntity()
    {
        // Arrange
        
        // Act
        var actual = Sphere.Create();

        // Assert
        actual.Should().BeAssignableTo<Entity>();
    }

    [Fact]
    public void CanCreateDefaultSphereOfGlass()
    {
        // Arrange
        
        // Act
        var actual = Sphere.Glass();

        // Assert
        actual.Transform.Should().BeEquivalentTo(Matrix.Identity);
        actual.Material.Transparency.Should().Be(1f);
        actual.Material.RefractionIndex.Should().Be(1.5f);
    }
}
