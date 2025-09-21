using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Operations;

namespace RayGround.Tests;

public class SphereTests
{
    [Fact]
    public void DefaultSphereTransformIsIdentity()
    {
        // Arrange
        var sphere   = Sphere.Create();
        var expected = RayMatrix.Identity;

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
        var ray    = Ray.Create(RayTuple.NewPoint(0, 0, -5), RayTuple.NewVector(0, 0, 1));
        var sphere = Sphere.Create();
        var scale  = Transform.Scaling(2, 2, 2);
        sphere     = sphere.Morph(scale);
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
        var ray    = Ray.Create(RayTuple.NewPoint(0, 0, -5), RayTuple.NewVector(0, 0, 1));
        var sphere = Sphere.Create();
        var scale  = Transform.Translation(5, 0, 0);
        sphere     = sphere.Morph(scale);
        
        // Act
        var actual = ray.Intersect(sphere);
        
        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void CorrectNormalOnSphereOnXAxis()
    {
        // Arrange
        var sphere   = Sphere.Create();
        var expected = RayTuple.NewVector(1, 0, 0);

        // Act
        var actual = sphere.NormalAt(RayTuple.NewPoint(1, 0, 0));

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CorrectNormalOnSphereOnYAxis()
    {
        // Arrange
        var sphere   = Sphere.Create();
        var expected = RayTuple.NewVector(0, 1, 0);
        
        // Act
        var actual = sphere.NormalAt(RayTuple.NewVector(0, 1, 0));
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CorrectNormalOnSphereOnZAxis()
    {
        // Arrange
        var sphere   = Sphere.Create();
        var expected = RayTuple.NewVector(0, 0, 1);
        
        // Act
        var actual = sphere.NormalAt(RayTuple.NewVector(0, 0, 1));
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CorrectNormalOnSphereAtNonAxial()
    {
        // Arrange
        var sphere   = Sphere.Create();
        var expected = RayTuple.NewVector(MathF.Sqrt(3)/3, MathF.Sqrt(3)/3, MathF.Sqrt(3)/3);
        
        // Act
        var actual = sphere.NormalAt(RayTuple.NewVector(MathF.Sqrt(3)/3, MathF.Sqrt(3)/3, MathF.Sqrt(3)/3));
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void NormalIsANormalizedVector()
    {
        // Arrange
        var sphere = Sphere.Create();
        
        // Act
        var actual = sphere.NormalAt(RayTuple.NewVector(MathF.Sqrt(3)/3, MathF.Sqrt(3)/3, MathF.Sqrt(3)/3));
        
        // Assert
        actual.Should().BeEquivalentTo(actual.Normalize());
    }

    [Fact]
    public void CanComputeCorrectNormalOnTranslatedSphere()
    {
        // Arrange
        var sphere   = Sphere.Create()
            .Morph(Transform.Translation(0, 1, 0));
        var expected = RayTuple.NewVector(0, 0.70711f, -0.70711f);

        // Act
        var actual = sphere.NormalAt(RayTuple.NewPoint(0, 1.70711f, -0.70711f));

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CanComputeCorrectNormalOnTransformedSphere()
    {
        // Arrange
        var sphere   = Sphere.Create()
            .Morph(Transform.Scaling(1, 0.5f, 1) * Transform.RotationZ(float.Pi/5));
        var expected = RayTuple.NewVector(0, .97014f, -.24254f);

        // Act
        var actual = sphere.NormalAt(RayTuple.NewPoint(0, MathF.Sqrt(2)/2, -MathF.Sqrt(2)/2));

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CreatedSpheresHaveADefaultMaterialAssigned()
    {
        // Arrange
        var expected = Material.Create();

        // Act
        var actual = Sphere.Create();

        // Assert
        actual.Material.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void SpheresCanBeAssignedAGivenMaterial()
    {
        // Arrange
        var sphere   = Sphere.Create();
        var material = Material.Create(ambient: 1);

        // Act
        var actual = sphere.Paint(material);

        // Assert
        actual.Material.Should().BeEquivalentTo(material);
    }
}
