using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Operations;

namespace RayGround.Tests;

public class RayTests
{
    [Fact]
    public void CanCreateAndQueryARay()
    {
        // Arrange
        var origin    = Fewple.NewPoint(1, 2, 3);
        var direction = Fewple.NewVector(4, 5, 6);

        // Act
        var actual = Ray.Create(origin, direction);

        // Assert
        actual.Origin.Should().BeEquivalentTo(origin);
        actual.Direction.Should().BeEquivalentTo(direction);
    }

    public static IEnumerable<object[]> PositionTheories =>
        new List<object[]>
        { new object[] {   0 , Fewple.NewPoint(2, 3, 4)    }
        , new object[] {   1 , Fewple.NewPoint(3, 3, 4)    }
        , new object[] {  -1 , Fewple.NewPoint(1, 3, 4)    }
        , new object[] { 2.5 , Fewple.NewPoint(4.5f, 3, 4) }
        };
    
    [Theory]
    [MemberData(nameof(PositionTheories))]
    public void CanComputePointFromDistance(float t, Fewple expected)
    {
        // Arrange
        var origin    = Fewple.NewPoint(2, 3, 4);
        var direction = Fewple.NewVector(1, 0, 0);
        var ray       = Ray.Create(origin, direction);

        // Act
        var actual = ray.Position(t);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void RayIntersectsSphereAtTwoPoints()
    {
        // Arrange
        var ray      = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
        var sphere   = Sphere.Unit();
        var expected = new[] { Intersection.Create(4.0f, sphere), Intersection.Create(6.0f, sphere) };

        // Act
        var actual = ray.Intersect(sphere);

        // Assert
        actual.Length.Should().Be(expected.Length);
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void RayIntersectsSphereAtTangent()
    {
        // Arrange
        var ray      = Ray.Create(Fewple.NewPoint(0, 1, -5), Fewple.NewVector(0, 0, 1));
        var sphere   = Sphere.Unit();
        var expected = new[] { Intersection.Create(5, sphere), Intersection.Create(5, sphere) };

        // Act
        var actual = ray.Intersect(sphere);

        // Assert
        actual.Length.Should().Be(expected.Length);
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void RayMissesSphere()
    {
        // Arrange
        var ray      = Ray.Create(Fewple.NewPoint(0, 2, -5), Fewple.NewVector(0, 0, 1));
        var sphere   = Sphere.Unit();
        var expected = Array.Empty<Intersection>();

        // Act
        var actual = ray.Intersect(sphere);

        // Assert
        actual.Length.Should().Be(expected.Length);
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void RayIntersectsSphereFromWithin()
    {
        // Arrange
        var ray      = Ray.Create(Fewple.NewPoint(0, 0, 0), Fewple.NewVector(0, 0, 1));
        var sphere   = Sphere.Unit();
        var expected = new[] { Intersection.Create(-1.0f, sphere), Intersection.Create(1.0f, sphere) };

        // Act
        var actual = ray.Intersect(sphere);

        // Assert
        actual.Length.Should().Be(expected.Length);
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void RayIntersectsSphereFromBehind()
    {
        // Arrange
        var ray      = Ray.Create(Fewple.NewPoint(0, 0, 5), Fewple.NewVector(0, 0, 1));
        var sphere   = Sphere.Unit();
        var expected = new[] { Intersection.Create(-6.0f, sphere), Intersection.Create(-4.0f, sphere) };

        // Act
        var actual = ray.Intersect(sphere);

        // Assert
        actual.Length.Should().Be(expected.Length);
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void RayIntersectionContainsCorrectObject()
    {
        // Arrange
        var ray      = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
        var sphere   = Sphere.Unit();

        // Act
        var actual = ray.Intersect(sphere);

        // Assert
        actual.Length.Should().Be(2);
        actual[0].Collided.Should().Be(sphere);
        actual[1].Collided.Should().Be(sphere);
    }

    [Fact]
    public void CanTranslateRay()
    {
        // Arrange
        var ray       = Ray.Create(Fewple.NewPoint(1, 2, 3), Fewple.NewVector(0, 1, 0));
        var translate = Transform.Translation(3, 4, 5);
        var expected  = Ray.Create(Fewple.NewPoint(4, 6, 8), Fewple.NewVector(0, 1, 0));

        // Act
        var actual = ray.Morph(translate);

        // Assert
        actual.Should().NotBe(ray);
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CanScaleRay()
    {
        // Arrange
        var ray      = Ray.Create(Fewple.NewPoint(1, 2, 3), Fewple.NewVector(0, 1, 0));
        var scale    = Transform.Scaling(2, 3, 4);
        var expected = Ray.Create(Fewple.NewPoint(2, 6, 12), Fewple.NewVector(0, 3, 0));

        // Act
        var actual = ray.Morph(scale);

        // Assert
        actual.Should().NotBe(ray);
        actual.Should().BeEquivalentTo(expected);
    }
}
