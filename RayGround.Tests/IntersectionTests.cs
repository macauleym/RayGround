using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Constants;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
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
        actual.Should().BeNull();
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
        var ray = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
        var shape = Sphere.Create();
        var intersection = Intersection.Create(4, shape);
        
        // Act
        var actual = intersection.Precompute(ray);

        // Assert
        actual.RayPoint.Should().Be(intersection.RayTime);
        actual.Collided.Should().BeEquivalentTo(intersection.Collided);
        actual.Point.Should().BeEquivalentTo(Fewple.NewPoint(0, 0, -1));
        actual.EyeVector.Should().BeEquivalentTo(Fewple.NewVector(0, 0, -1));
        actual.NormalVector.Should().BeEquivalentTo(Fewple.NewVector(0, 0, -1));
    }

    [Fact]
    public void PrecomputationSetsInsideFalseWhenHitOccursOutside()
    {
        // Arrange
        var ray          = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
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
        var ray          = Ray.Create(Fewple.NewPoint(0, 0, 0), Fewple.NewVector(0, 0, 1));
        var shape        = Sphere.Create();
        var intersection = Intersection.Create(1, shape);

        // Act
        var actual = intersection.Precompute(ray);

        // Assert
        actual.IsInside.Should().BeTrue();
        actual.Point.Should().BeEquivalentTo(Fewple.NewPoint(0, 0, 1));
        actual.EyeVector.Should().BeEquivalentTo(Fewple.NewVector(0, 0, -1));
        // Normally would be (0,0,1), but we invert due to being within the shape.
        actual.NormalVector.Should().BeEquivalentTo(Fewple.NewVector(0, 0, -1));
    }

    [Fact]
    public void HitShouldOffsetThePoint()
    {
        // Arrange
        var ray = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
        var shape = Sphere
            .Create()
            .Morph(Transform.Translation(0, 0, 1));
        var intersection = Intersection.Create(5, shape);

        // Act
        var actual = intersection.Precompute(ray);

        // Assert
        actual.OverPoint.Z.Should().BeLessThan(Floating.Epsilon / -2);
        actual.Point.Z.Should().BeGreaterThan(actual.OverPoint.Z);
    }

    [Fact]
    public void ReflectionsCanBePrecomputed()
    {
        // Arrange
        var entity       = Plane.Create();
        var ray          = Ray.Create(Fewple.NewPoint(0, 1, -1), Fewple.NewVector(0, -Floating.Root2Over2, Floating.Root2Over2));
        var intersection = Intersection.Create(Floating.Root2Over2, entity);

        // Act
        var actual = intersection.Precompute(ray);

        // Assert
        actual.ReflectVector.Should().BeEquivalentTo(Fewple.NewVector(0, Floating.Root2Over2, Floating.Root2Over2));
    }

    [Theory]
    [InlineData("Air to A" , 0 , 1.000293f , 1.5f      )]
    [InlineData("A to B"   , 1 , 1.5f      , 2         )]
    [InlineData("B to C"   , 2 , 2         , 2.5f      )]
    [InlineData("C to B"   , 3 , 2.5f      , 2.5f      )]
    [InlineData("C to A"   , 4 , 2.5f      , 1.5f      )]
    [InlineData("A to Air" , 5 , 1.5f      , 1.000293f )]
    public void CorrectRefractionPointsFoundAtIntersections(string name
    , int index
    , float n1
    , float n2
    ) {
        // Arrange
        var glassSphereA = Sphere
            .Glass()
            .Morph(Transform.Scaling(2, 2, 2))
            .Paint(Material.Create(refractionIndex: 1.5f));
        var internalGlassB = Sphere
            .Glass()
            .Morph(Transform.Translation(0, 0, -0.25f))
            .Paint(Material.Create(refractionIndex: 2f));
        var internalGlassC = Sphere
            .Glass()
            .Morph(Transform.Translation(0, 0, 0.25f))
            .Paint(Material.Create(refractionIndex: 2.5f));
        var ray = Ray.Create(Fewple.NewPoint(0, 0, -4), Fewple.NewVector(0, 0, 1));
        var intersections = new[]
        { Intersection.Create(2f   , glassSphereA  )
        , Intersection.Create(2.75f, internalGlassB)
        , Intersection.Create(3.25f, internalGlassC)
        , Intersection.Create(4.75f, internalGlassB)
        , Intersection.Create(5.25f, internalGlassC)
        , Intersection.Create(6f   , glassSphereA  ) 
        };

        // Act
        var actual = intersections[index].Precompute(ray, intersections);

        // Assert
        actual.RefractionFrom.Should().Be(n1);
        actual.RefractionTo.Should().Be(n2);
    }

    [Fact]
    public void CanComputeOffsetPointBelowHitSurface()
    {
        // Arrange
        var ray    = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
        var sphere = Sphere.Glass()
            .Morph(Transform.Translation(0, 0, 1));
        var intersection = Intersection.Create(5, sphere);

        // Act
        var actual = intersection.Precompute(ray);

        // Assert
        actual.UnderPoint.Z.Should().BeGreaterThan(Floating.Epsilon / 2);
        actual.Point.Z.Should().BeLessThan(actual.UnderPoint.Z);
    }

    [Fact]
    public void CanCalculateSchlickApproximationForTotalInternalReflectance()
    {
        // Arrange
        var shpere        = Sphere.Glass();
        var ray           = Ray.Create(Fewple.NewPoint(0, 0, Floating.Root2Over2), Fewple.NewVector(0, 1, 0));
        var intersections = new[]
        { Intersection.Create(-Floating.Root2Over2, shpere)
        , Intersection.Create(Floating.Root2Over2, shpere) 
        };
        var precomputed = intersections[1].Precompute(ray, intersections);

        // Act
        var actual = precomputed.Schlick();

        // Assert
        actual.Should().Be(1);
    }

    [Fact]
    public void SchlickApproximationSlightWhenPerpendicular()
    {
        // Arrange
        var sphere        = Sphere.Glass();
        var ray           = Ray.Create(Fewple.NewPoint(0, 0, 0), Fewple.NewVector(0, 1, 0));
        var intersections = new[] { Intersection.Create(-1, sphere), Intersection.Create(1, sphere) };
        var precomputed   = intersections[1].Precompute(ray, intersections);

        // Act
        var actual = precomputed.Schlick();

        // Assert
        actual.Should().BeApproximately(0.04f, Floating.Epsilon);
    }

    [Fact]
    public void SchlickSignificantWhenVerySmallAngle()
    {
        // Arrange
        var sphere        = Sphere.Glass();
        var ray           = Ray.Create(Fewple.NewPoint(0, 0.99f, -2), Fewple.NewVector(0, 0, 1));
        var intersections = new[] { Intersection.Create(1.8589f, sphere) };
        var precomputed   = intersections[0].Precompute(ray, intersections);

        // Act
        var actual = precomputed.Schlick();

        // Assert
        actual.Should().BeApproximately(0.48873f, Floating.Epsilon);
    }
}
