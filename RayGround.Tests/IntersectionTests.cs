using FluentAssertions;
using Microsoft.VisualBasic;
using RayGround.Core;
using RayGround.Core.Constants;
using RayGround.Core.Extensions;
using RayGround.Core.Operations;

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
        actual.RayTime.Should().Be(t);
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
        actual[0].RayTime.Should().Be(first.RayTime);
        actual[1].RayTime.Should().Be(second.RayTime);
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
    public void HitIsNothingWhenAllIntersectionsNegative()
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

    [Fact]
    public void IntersectionCanBePrecomputed()
    {
        // Arrange
        var ray = Ray.Create(RayTuple.NewPoint(0, 0, -5), RayTuple.NewVector(0, 0, 1));
        var shape = Sphere.Create();
        var intersection = Intersection.Create(4, shape);
        
        // Act
        var actual = intersection.Precompute(ray);

        // Assert
        actual.RayPoint.Should().Be(intersection.RayTime);
        actual.Collided.Should().BeEquivalentTo(intersection.Collided);
        actual.Point.Should().BeEquivalentTo(RayTuple.NewPoint(0, 0, -1));
        actual.EyeVector.Should().BeEquivalentTo(RayTuple.NewVector(0, 0, -1));
        actual.NormalVector.Should().BeEquivalentTo(RayTuple.NewVector(0, 0, -1));
    }

    [Fact]
    public void PrecomputationSetsInsideFalseWhenHitOccursOutside()
    {
        // Arrange
        var ray          = Ray.Create(RayTuple.NewPoint(0, 0, -5), RayTuple.NewVector(0, 0, 1));
        var shape        = Sphere.Create();
        var intersection = Intersection.Create(4, shape);

        // Act
        var actual = intersection.Precompute(ray);

        // Assert
        actual.IsInside.Should().BeFalse();
    }

    [Fact]
    public void PrecomputationSetsInsideTrueWhenHitOccursInside()
    {
        // Arrange
        var ray          = Ray.Create(RayTuple.NewPoint(0, 0, 0), RayTuple.NewVector(0, 0, 1));
        var shape        = Sphere.Create();
        var intersection = Intersection.Create(1, shape);

        // Act
        var actual = intersection.Precompute(ray);

        // Assert
        actual.IsInside.Should().BeTrue();
        actual.Point.Should().BeEquivalentTo(RayTuple.NewPoint(0, 0, 1));
        actual.EyeVector.Should().BeEquivalentTo(RayTuple.NewVector(0, 0, -1));
        // Normally would be (0,0,1), but we invert due to being within the shape.
        actual.NormalVector.Should().BeEquivalentTo(RayTuple.NewVector(0, 0, -1));
    }

    [Fact]
    public void HitShouldOffsetThePoint()
    {
        // Arrange
        var ray = Ray.Create(RayTuple.NewPoint(0, 0, -5), RayTuple.NewVector(0, 0, 1));
        var shape = Sphere.Create()
            .Morph(Transform.Translation(0, 0, 1));
        var intersection = Intersection.Create(5, shape);

        // Act
        var actual = intersection.Precompute(ray);

        // Assert
        actual.OverPoint.Z.Should().BeLessThan(FloatingPoint.Epsilon / -2);
        actual.Point.Z.Should().BeGreaterThan(actual.OverPoint.Z);
    }
}
