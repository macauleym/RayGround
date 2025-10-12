using FluentAssertions;
using FluentAssertions.Equivalency;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Operations;

namespace RayGround.Tests;

public class WorldTests
{
    [Fact]
    public void DefaultWorldContainsNoObjectsAndNoLight()
    {
        // Arrange
        
        // Act
        var actual = World.Empty();

        // Assert
        actual.Lights.Should().BeEmpty();
        actual.Shapes.Should().BeEmpty();
    }

    [Fact]
    public void StarterWorldContainsTwoSpheresAndOneLight()
    {
        // Arrange
        var light   = Light.Create(RayTuple.NewPoint(-10, 10, -10), RayColor.Create(1, 1, 1));
        var sphere1 = Sphere.Create()
            .Paint(Material.Create(
              diffuse: 0.7f
            , specular: 0.2f
            , color: RayColor.Create(0.8f, 1f, 0.6f)
            ));
        var sphere2 = Sphere.Create()
            .Morph(Transform.Scaling(0.5f, 0.5f, 0.5f));

        // Act
        var actual = World.Default();
       
        // Assert
        Func<EquivalencyOptions<Sphere>,EquivalencyOptions<Sphere>> excludeIdOption = o => o.Excluding(s => s.Id);
        actual.Lights.Should().ContainEquivalentOf(light);
        actual.Shapes.Should().ContainEquivalentOf(sphere1, excludeIdOption);
        actual.Shapes.Should().ContainEquivalentOf(sphere2, excludeIdOption);
    }

    [Fact]
    public void RayCanIntersectAWorld()
    {
        // Arrange
        var world    = World.Default();
        var ray      = Ray.Create(RayTuple.NewPoint(0, 0, -5), RayTuple.NewVector(0, 0, 1));
        var expected = 4;

        // Act
        var actual = ray.IntersectWorld(world);

        // Assert
        actual.Length.Should().Be(expected);
        actual[0].RayTime.Should().Be(4);
        actual[1].RayTime.Should().Be(4.5f);
        actual[2].RayTime.Should().Be(5.5f);
        actual[3].RayTime.Should().Be(6);
    }

    [Fact]
    public void ShadingAtIntersection()
    {
        // Arrange
        var world        = World.Default();
        var ray          = Ray.Create(RayTuple.NewPoint(0, 0, -5), RayTuple.NewVector(0, 0, 1));
        var shape        = world.Shapes.First();
        var intersection = Intersection.Create(4f, shape);
        var comps        = intersection.Precompute(ray);
        var expected     = RayColor.Create(0.38066f, 0.47583f, 0.2855f);

        // Act
        var actual = world.ShadeHit(comps);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ShadingIntersectionFromInside()
    {
        // Arrange
        var world       = World.Default();
        var light       = Light.Create(RayTuple.NewPoint(0, 0.25f, 0), RayColor.Create(1f, 1f, 1f));
        world.Lights[0] = light;
        
        var ray          = Ray.Create(RayTuple.NewPoint(0, 0, 0), RayTuple.NewVector(0, 0, 1));
        var shape        = world.Shapes[1];
        var intersection = Intersection.Create(0.5f, shape);
        var comps        = intersection.Precompute(ray);
        
        var expected = RayColor.Create(0.90498f, 0.90498f, 0.90498f);

        // Act
        var actual = world.ShadeHit(comps);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void DefaultColorBlackWhenRayMisses()
    {
        // Arrange
        var world    = World.Default();
        var ray      = Ray.Create(RayTuple.NewPoint(0, 0, -5), RayTuple.NewVector(0, 1, 0));
        var expected = RayColor.Create(0, 0, 0);

        // Act
        var actual = world.ColorAt(ray);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CorrectColorCalculatedAtRayHit()
    {
        // Arrange
        var world    = World.Default();
        var ray      = Ray.Create(RayTuple.NewPoint(0, 0, -5), RayTuple.NewVector(0, 0, 1));
        var expected = RayColor.Create(0.38066f, 0.47583f, 0.2855f);

        // Act
        var actual = world.ColorAt(ray);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ColorWhenIntersectionBehindRay()
    {
        // Arrange
        var world       = World.Default();
        var outer       = world.Shapes[0];
        world.Shapes[0] = outer.Paint(Material.Create(ambient: 1f));
        var inner       = world.Shapes[1];
        world.Shapes[1] = inner.Paint(Material.Create(ambient: 1f));
        
        var ray = Ray.Create(RayTuple.NewPoint(0, 0, 0.75f), RayTuple.NewVector(0, 0, -1));
        
        var expected = inner.Material.Color;

        // Act
        var actual = world.ColorAt(ray);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void NoShadowWhenNothingCollinearWithPointAndLight()
    {
        // Arrange
        var world = World.Default();
        var point = RayTuple.NewPoint(0, 10, 0);

        // Act
        var actual = Illuminate.IsShadowed(world.Lights.First(), point, world.Shapes);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void ShadowedWhenObjectBetweenPointAndLight()
    {
        // Arrange
        var world = World.Default();
        var point = RayTuple.NewPoint(10, -10, 10);
        
        // Act
        var actual = Illuminate.IsShadowed(world.Lights.First(), point, world.Shapes);
        
        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void NoShadowWhenPointIsBehindLight()
    {
        // Arrange
        var world = World.Default();
        var point = RayTuple.NewPoint(-20, 20, -20);

        // Act
        var actual = Illuminate.IsShadowed(world.Lights.First(), point, world.Shapes);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void NoShadowWhenObjectIsBehindPoint()
    {
        // Arrange
        var world = World.Default();
        var point = RayTuple.NewPoint(-2, 2, -2);

        // Act
        var actual = Illuminate.IsShadowed(world.Lights.First(), point, world.Shapes);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void ShadeHitHasIntersectionInShadow()
    {
        // Arrange
        var light   = Light.Create(RayTuple.NewPoint(0, 0, -10), RayColor.Create(1, 1, 1));
        var sphere1 = Sphere.Create();
        var sphere2 = sphere1.Morph(Transform.Translation(0, 0, 10));
        var world   = World.Create([light], [sphere1, sphere2]);
        
        var ray          = Ray.Create(RayTuple.NewPoint(0, 0, 5), RayTuple.NewVector(0, 0, 1));
        var intersection = Intersection.Create(4, sphere2);
        var comps        = intersection.Precompute(ray);

        var expected = RayColor.Uniform(sphere2.Material.Ambient);

        // Act
        var actual = world.ShadeHit(comps);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
