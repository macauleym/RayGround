using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Models;
using Plane = RayGround.Core.Plane;

namespace RayGround.Tests;

public class PlaneTests
{
    [Fact]
    public void PlaneNormalIsConstantEverywhere()
    {
        // Arrange
        var plane = Plane.Default();

        // Act
        var actual = new List<Fewple>
        { plane.LocalNormal(Fewple.NewPoint(0, 0, 0))
        , plane.LocalNormal(Fewple.NewPoint(10, 0, -10))
        , plane.LocalNormal(Fewple.NewPoint(-5, 0, 150))
        };

        // Assert
        actual.Should().AllBeEquivalentTo(Fewple.NewVector(0, 1, 0));
    }

    [Fact]
    public void NoIntersectionsWhenRayIsParallel()
    {
        // Arrange
        var plane = Plane.Default();
        var ray   = Ray.Create(Fewple.NewPoint(0, 10, 0), Fewple.NewVector(0, 0, 1));

        // Act
        var actual = plane.Intersections(ray);

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void NoIntersectionsWhenRayIsCoplanar()
    {
        // Arrange
        var plane = Plane.Default();
        var ray   = Ray.Create(Fewple.NewPoint(0, 0, 0), Fewple.NewVector(0, 0, 1));

        // Act
        var actual = plane.Intersections(ray);

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void RayCanIntersectFromAbove()
    {
        // Arrange
        var plane = Plane.Default();
        var ray   = Ray.Create(Fewple.NewPoint(0, 1, 0), Fewple.NewVector(0, -1, 0));

        // Act
        var actual = plane.Intersections(ray);

        // Assert
        actual.Length.Should().Be(1);
        actual[0].Should().Be(1);
    }
    
    [Fact]
    public void RayCanIntersectFromBelow()
    {
        // Arrange
        var plane = Plane.Default();
        var ray   = Ray.Create(Fewple.NewPoint(0, -1, 0), Fewple.NewVector(0, 1, 0));

        // Act
        var actual = plane.Intersections(ray);

        // Assert
        actual.Length.Should().Be(1);
        actual[0].Should().Be(1);
    }
}
