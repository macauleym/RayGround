using FluentAssertions;
using FluentAssertions.Equivalency;
using RayGround.Core;
using RayGround.Core.Constants;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Models.Entities;
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
        actual.Entities.Should().BeEmpty();
    }

    [Fact]
    public void StarterWorldContainsTwoSpheresAndOneLight()
    {
        // Arrange
        var light   = Light.Create(Fewple.NewPoint(-10, 10, -10), Color.Create(1, 1, 1));
        var sphere1 = Sphere.Create()
            .Paint(Material.Create(
                  diffuse: 0.7f
                , specular: 0.2f)
                .Bucket(Color.Create(0.8f, 1f, 0.6f)));
        var sphere2 = Sphere.Create()
            .Morph(Transform.Scaling(0.5f, 0.5f, 0.5f));

        // Act
        var actual = World.Default();
       
        // Assert
        Func<EquivalencyOptions<Entity>,EquivalencyOptions<Entity>> excludeIdOption = o => o.Excluding(s => s.Id);
        actual.Lights.Should().ContainEquivalentOf(light);
        actual.Entities.Should().ContainEquivalentOf(sphere1, excludeIdOption);
        actual.Entities.Should().ContainEquivalentOf(sphere2, excludeIdOption);
    }

    [Fact]
    public void RayCanIntersectAWorld()
    {
        // Arrange
        var world    = World.Default();
        var ray      = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
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
        var ray          = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
        var shape        = world.Entities.First();
        var intersection = Intersection.Create(4f, shape);
        var comps        = intersection.Precompute(ray);
        var expected     = Color.Create(0.38066f, 0.47583f, 0.2855f);

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
        var light       = Light.Create(Fewple.NewPoint(0, 0.25f, 0), Color.Create(1f, 1f, 1f));
        world.Lights[0] = light;
        
        var ray          = Ray.Create(Fewple.NewPoint(0, 0, 0), Fewple.NewVector(0, 0, 1));
        var shape        = world.Entities[1];
        var intersection = Intersection.Create(0.5f, shape);
        var comps        = intersection.Precompute(ray);
        
        var expected = Color.Create(0.90498f, 0.90498f, 0.90498f);

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
        var ray      = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 1, 0));
        var expected = Color.Create(0, 0, 0);

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
        var ray      = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
        var expected = Color.Create(0.38066f, 0.47583f, 0.2855f);

        // Act
        var actual = world.ColorAt(ray);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ColorWhenIntersectionBehindRay()
    {
        // Arrange
        var world         = World.Default();
        var outer         = world.Entities[0];
        world.Entities[0] = outer.Paint(Material.Create(ambient: 1f));
        var inner         = world.Entities[1];
        world.Entities[1] = inner.Paint(Material.Create(ambient: 1f));
        
        var ray = Ray.Create(Fewple.NewPoint(0, 0, 0.75f), Fewple.NewVector(0, 0, -1));
        
        var expected = inner.Material.Pattern.Primary;

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
        var point = Fewple.NewPoint(0, 10, 0);

        // Act
        var actual = Illuminate.IsShadowed(world.Lights.First(), point, world.Entities);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void ShadowedWhenObjectBetweenPointAndLight()
    {
        // Arrange
        var world = World.Default();
        var point = Fewple.NewPoint(10, -10, 10);
        
        // Act
        var actual = Illuminate.IsShadowed(world.Lights.First(), point, world.Entities);
        
        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void NoShadowWhenPointIsBehindLight()
    {
        // Arrange
        var world = World.Default();
        var point = Fewple.NewPoint(-20, 20, -20);

        // Act
        var actual = Illuminate.IsShadowed(world.Lights.First(), point, world.Entities);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void NoShadowWhenObjectIsBehindPoint()
    {
        // Arrange
        var world = World.Default();
        var point = Fewple.NewPoint(-2, 2, -2);

        // Act
        var actual = Illuminate.IsShadowed(world.Lights.First(), point, world.Entities);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void ShadeHitHasIntersectionInShadow()
    {
        // Arrange
        var light   = Light.Create(Fewple.NewPoint(0, 0, -10), Color.Create(1, 1, 1));
        var sphere1 = Sphere.Create();
        var sphere2 = Sphere.Create().Morph(Transform.Translation(0, 0, 10));
        var world   = World.Create([light], [sphere1, sphere2]);
        
        var ray          = Ray.Create(Fewple.NewPoint(0, 0, 5), Fewple.NewVector(0, 0, 1));
        var intersection = Intersection.Create(4, sphere2);
        var comps        = intersection.Precompute(ray);

        var expected = Color.Uniform(sphere2.Material.Ambient);

        // Act
        var actual = world.ShadeHit(comps);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void BlackIsColorReflectedForNonReflectiveMaterial()
    {
        // Arrange
        var world = World.Default();
        world.Entities[1].Paint(Material.Create(ambient: 1));
        
        var ray          = Ray.Create(Fewple.NewPoint(0, 0, 0), Fewple.NewVector(0, 0, 1));
        var intersection = Intersection.Create(1, world.Entities[1]);
        var precomputed  = intersection.Precompute(ray);

        // Act
        var actual = world.ReflectedColor(precomputed);

        // Assert
        actual.Should().BeEquivalentTo(Color.Black);
    }

    [Fact]
    public void CorrectColorIsReflectedForReflectiveMaterial()
    {
        // Arrange
        var world           = World.Default();
        var reflectivePlane = Plane
            .Create()
            .Morph(Transform.Translation(0, -1, 0))
            .Paint(Material.Create(reflective: 0.5f));
        world.Entities.Add(reflectivePlane);
        var ray          = Ray.Create(Fewple.NewPoint(0, 0, -3), Fewple.NewVector(0, -Floating.Root2Over2, Floating.Root2Over2));
        var intersection = Intersection.Create(Floating.Root2, reflectivePlane);
        var prepared     = intersection.Precompute(ray);

        // Act
        var actual = world.ReflectedColor(prepared);

        // Assert
        actual.Should().BeEquivalentTo(Color.Create(0.19032f, 0.2379f, 0.14274f));
    }

    [Fact]
    public void ShadingCorrectlyAddsReflectedColorToFinalColor()
    {
        // Arrange
        var world           = World.Default();
        var reflectivePlane = Plane
            .Create()
            .Morph(Transform.Translation(0, -1, 0))
            .Paint(Material.Create(reflective: 0.5f));
        world.Entities.Add(reflectivePlane);
        var ray          = Ray.Create(Fewple.NewPoint(0, 0, -3), Fewple.NewVector(0, -Floating.Root2Over2, Floating.Root2Over2));
        var intersection = Intersection.Create(Floating.Root2, reflectivePlane);
        var prepared     = intersection.Precompute(ray);

        // Act
        var actual = world.ShadeHit(prepared);

        // Assert
        actual.Should().BeEquivalentTo(Color.Create(0.87677f, 0.92436f, 0.82918f));
    }

    [Fact]
    public void CanColorMutuallyReflectiveSurfaces()
    {
        // Arrange
        var world = World.Default();
        var lower = Plane
            .Create()
            .Morph(Transform.Translation(0, -1, 0))
            .Paint(Material.Create(reflective: 1f));
        var upper = Plane
            .Create() 
            .Morph(Transform.Translation(0, 1, 0))
            .Paint(Material.Create(reflective: 1f));
        var ray = Ray.Create(Fewple.NewPoint(0, 0, 0), Fewple.NewVector(0, 1, 0));
        
        world.Lights.Add(Light.Create(Fewple.NewPoint(0,0,0), Color.White));
        world.Entities.AddRange([lower, upper]);
        
        // Act
        var actual = world.ColorAt(ray);

        // Assert
        actual.Should().BeAssignableTo<Color>();
    }

    [Fact]
    public void ReflectionHasMaximumRecursiveDepth()
    {
        // Arrange
        var world = World.Default();
        var plane = Plane
            .Create()
            .Paint(Material.Create(reflective: 0.5f))
            .Morph(Transform.Translation(0, -1, 0));
        world.Entities.Add(plane);
        var ray          = Ray.Create(Fewple.NewPoint(0, 0, -3), Fewple.NewVector(0, -Floating.Root2Over2, Floating.Root2Over2));
        var intersection = Intersection.Create(Floating.Root2, plane);
        var precomputed  = intersection.Precompute(ray);

        // Act
        var actual = world.ReflectedColor(precomputed, 0);

        // Assert
        actual.Should().BeEquivalentTo(Color.Black);
    }

    [Fact]
    public void ColorOfOpaqueRefractionIsBlack()
    {
        // Arrange
        var world         = World.Default();
        var entity        = world.Entities[0];
        var ray           = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
        var intersections = new[] { Intersection.Create(4, entity), Intersection.Create(6, entity) };
        var precomputed   = intersections[0].Precompute(ray, intersections);

        // Act
        var actual = world.RefractedColor(precomputed, 5);

        // Assert
        actual.Should().BeEquivalentTo(Color.Black);
    }

    [Fact]
    public void RefractedColorAtMaximumDepthIsBlack()
    {
        // Arrange
        var world  = World.Default();
        var entity = world.Entities[0]
            .Paint(Material.Create(transparency: 1, refractionIndex: 1.5f));
        var ray           = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
        var intersections = new[] { Intersection.Create(4, entity), Intersection.Create(6, entity) };
        var precomputed   = intersections[0].Precompute(ray, intersections);

        // Act
        var actual = world.RefractedColor(precomputed, 0);

        // Assert
        actual.Should().BeEquivalentTo(Color.Black);
    }

    [Fact]
    public void TotalInternalReflectionReturnsBlack()
    {
        // Arrange
        var world  = World.Default();
        var entity = world.Entities[0]
            .Paint(Material.Create(transparency: 1, refractionIndex: 1.5f));
        var ray           = Ray.Create(Fewple.NewPoint(0, 0, Floating.Root2Over2), Fewple.NewVector(0, 1, 0));
        var intersections = new[]
            { Intersection.Create(-Floating.Root2Over2, entity)
            , Intersection.Create(Floating.Root2Over2, entity)
            };
        
        var precomputed = intersections[1].Precompute(ray, intersections);

        // Act
        var actual = world.RefractedColor(precomputed, 5);

        // Assert
        actual.Should().BeEquivalentTo(Color.Black);
    }

    [Fact]
    public void RefractionRayHasCorrectColor()
    {
        // Arrange
        var world   = World.Default();
        var entityA = world.Entities[0];
        var entityB = world.Entities[1];
        entityA.Paint(Material
            .Create(ambient: 1)
            .Etch(TestPattern.Create()));
        entityB.Paint(Material
            .Create(transparency: 1, refractionIndex: 1.5f));

        var ray           = Ray.Create(Fewple.NewPoint(0, 0, 0.1f), Fewple.NewVector(0, 1, 0));
        var intersections = new[]
            { Intersection.Create(-0.9899f, entityA)
            , Intersection.Create(-0.4899f, entityB)
            , Intersection.Create(0.4899f, entityB)
            , Intersection.Create(0.9899f, entityA) 
            };
        
        var precomputed = intersections[2].Precompute(ray, intersections);

        // Act
        var actual = world.RefractedColor(precomputed, 5);

        // Assert
        actual.Should().BeEquivalentTo(Color.Create(0, 0.99888f, 0.04725f));
    }

    [Fact]
    public void CanShadeATransparentMaterial()
    {
        // Arrange
        var world = World.Default();
        var floor = Plane
            .Create()
            .Morph(Transform.Translation(0, -1, 0))
            .Paint(Material.Create(transparency: 0.5f, refractionIndex: 1.5f));
        var ball = Sphere
            .Create()
            .Morph(Transform.Translation(0, -3.5f, -0.5f))
            .Paint(Material
                .Create(ambient: 0.5f)
                .Bucket(Color.Create(1, 0, 0)));
        world.Entities.AddRange([floor, ball]);
        var ray           = Ray.Create(Fewple.NewPoint(0, 0, -3), Fewple.NewVector(0, -Floating.Root2Over2, Floating.Root2Over2));
        var intersections = new[] { Intersection.Create(Floating.Root2, floor) };
        var precomputed   = intersections[0].Precompute(ray, intersections);

        // Act
        var actual = world.ShadeHit(precomputed, 5);

        // Assert
        actual.Should().BeEquivalentTo(Color.Create(0.93642f, 0.68642f, 0.68642f));
    }

    [Fact]
    public void CanShadeAReflectiveTransparentMaterial()
    {
        // Arrange
        var world = World.Default();
        var ray   = Ray.Create(Fewple.NewPoint(0, 0, -3), Fewple.NewVector(0, -Floating.Root2Over2, Floating.Root2Over2));
        var floor = Plane
            .Create()
            .Morph(Transform.Translation(0, -1, 0))
            .Paint(Material.Create(
                  reflective: 0.5f
                , transparency: 0.5f
                , refractionIndex: 1.5f
                ));
        var ball = Sphere
            .Create()
            .Morph(Transform.Translation(0, -3.5f, -0.5f))
            .Paint(Material
                .Create(ambient: 0.5f)
                .Bucket(Color.Create(1, 0, 0)));
        world.Entities.AddRange([floor, ball]);

        var intersections = new[] { Intersection.Create(Floating.Root2, floor) };
        var precomputed   = intersections[0].Precompute(ray, intersections);

        // Act
        var actual = world.ShadeHit(precomputed, 5);

        // Assert
        actual.Should().BeEquivalentTo(Color.Create(0.93391f, 0.69643f, 0.69243f));
    }
}
