using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Extensions;

namespace RayGround.Tests;

public class IntersectionTests
{
    [Fact]
    public void IntersectionContainsTAndObject()
    {
        // Arrange
        var sphere = Sphere.Create();
        var t      = 3.5f;

        // Act
        var actual = Intersection.Create(t, sphere);

        // Assert
        actual.RayPoint.Should().Be(t);
        actual.Collided.Should().Be(sphere);
    }

    [Fact]
    public void CanAggregateIntersectionsIntoArray()
    {
        // Arrange
        var sphere = Sphere.Create();
        var first  = Intersection.Create(1, sphere);
        var second = Intersection.Create(2, sphere);
        
        // Act
        Intersection[] actual = [first, second];
        
        // Assert
        actual.Length.Should().Be(2);
        actual[0].RayPoint.Should().Be(first.RayPoint);
        actual[1].RayPoint.Should().Be(second.RayPoint);
    }

    [Fact]
    public void HitIsLowestPositiveWhenAllIntersectionsPositive()
    {
        // Arrange
        var sphere = Sphere.Create();
        var first  = Intersection.Create(1, sphere);
        var second = Intersection.Create(2, sphere);
        Intersection[] intersections = [first, second];

        // Act
        var actual = intersections.Hit();

        // Assert
        actual.Should().Be(first);
    }
    
    [Fact]
    public void HitIsLowestPositiveWhenSomeIntersectionsNegative()
    {
        // Arrange
        var sphere = Sphere.Create();
        var first  = Intersection.Create(-1, sphere);
        var second = Intersection.Create(1, sphere);
        Intersection[] intersections = [first, second];

        // Act
        var actual = intersections.Hit();

        // Assert
        actual.Should().Be(second);
    }
    
    [Fact]
    public void HitIsLowestPositiveWhenAllIntersectionsNegative()
    {
        // Arrange
        var sphere = Sphere.Create();
        var first  = Intersection.Create(-1, sphere);
        var second = Intersection.Create(-2, sphere);
        Intersection[] intersections = [first, second];

        // Act
        var actual = intersections.Hit();

        // Assert
        actual.HasValue.Should().BeFalse();
    }
    
    [Fact]
    public void HitIsAlwaysLowestPositiveNonNegativeIntersection()
    {
        // Arrange
        var sphere = Sphere.Create();
        var first  = Intersection.Create(5, sphere);
        var second = Intersection.Create(7, sphere);
        var third  = Intersection.Create(-3, sphere);
        var fourth = Intersection.Create(2, sphere);
        Intersection[] intersections = [first, second, third, fourth];

        // Act
        var actual = intersections.Hit();

        // Assert
        actual.Should().Be(fourth);
    }
}
