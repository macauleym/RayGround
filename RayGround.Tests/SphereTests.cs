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
        var sphere = Sphere.Create();
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
        var sphere = Sphere.Create();
        var translate = Transform.Translation(2, 3, 4);
        
        // Act
        var actual = sphere.UpdateTransform(translate);
        
        // Assert
        actual.Transform.Should().BeEquivalentTo(translate);
    }

    [Fact]
    public void IntersectAScaledSphereWithRay()
    {
        // Arrange
        var ray = Ray.Create(RayTuple.NewPoint(0, 0, -5), RayTuple.NewVector(0, 0, 1));
        var sphere = Sphere.Create();
        var scale = Transform.Scaling(2, 2, 2);
        sphere = sphere.UpdateTransform(scale);
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
        var ray = Ray.Create(RayTuple.NewPoint(0, 0, -5), RayTuple.NewVector(0, 0, 1));
        var sphere = Sphere.Create();
        var scale = Transform.Translation(5, 0, 0);
        sphere = sphere.UpdateTransform(scale);
        
        // Act
        var actual = ray.Intersect(sphere);
        
        // Assert
        actual.Should().BeEmpty();
    }
}
