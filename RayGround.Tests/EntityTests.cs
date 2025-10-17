using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Operations;

namespace RayGround.Tests;

public class EntityTests
{
    class TestEntity : Entity
    {
        public Ray SavedRay { get; private set; }
        
        public TestEntity(Fewple origin, Matrix transform, Material material, Guid id) 
        : base(origin, transform, material, id)
        { }

        public override float[] Intersections(Ray traced)
        {
            SavedRay = traced;
            
            return [0, 0];
        }

        public override Fewple LocalNormal(Fewple at) =>
            Fewple.NewVector(at.X, at.Y, at.Z);
    }

    static TestEntity CreateTestEntity() =>
        new ( Fewple.NewPoint(0, 0, 0)
            , Matrix.Identity
            , Material.Create()
            , Guid.NewGuid()
            );
    
    [Fact]
    public void DefaultTransformIsIdentity()
    {
        // Arrange
        var entity = CreateTestEntity();
        
        // Act
        var actual = entity.Transform;

        // Assert
        actual.Should().BeEquivalentTo(Matrix.Identity);
    }

    [Fact]
    public void CanModifyAnEntityTransform()
    {
        // Arrange
        var entity    = CreateTestEntity();
        var translate = Transform.Translation(2, 3, 4);

        // Act
        var actual = entity.Morph(translate);

        // Assert
        actual.Transform.Should().BeEquivalentTo(translate);
    }

    [Fact]
    public void StartsWithDefaultMaterial()
    {
        // Arrange
        var entity = CreateTestEntity();
        var expected = Material.Create();

        // Act
        var actual = entity.Material;

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CanAssignAMaterial()
    {
        // Arrange
        var entity   = CreateTestEntity();
        var expected = Material.Create(ambient: 1);

        // Act
        var actual = entity.Paint(expected);

        // Assert
        actual.Material.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void RayCorrectlyIntersectsWithScaledEntity()
    {
        // Arrange
        var ray = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
        var entity = CreateTestEntity()
            .Morph(Transform.Scaling(2, 2, 2))
            .As<TestEntity>();
        var expected = Ray.Create(Fewple.NewPoint(0, 0, -2.5f), Fewple.NewVector(0, 0, 0.5f));
        
        // Act
        ray.Intersect(entity);

        // Assert
        entity.SavedRay.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void RayCorrectlyIntersectsWithTranslatedEntity()
    {
        // Arrange
        var ray = Ray.Create(Fewple.NewPoint(0, 0, -5), Fewple.NewVector(0, 0, 1));
        var entity = CreateTestEntity()
            .Morph(Transform.Translation(5, 0, 0))
            .As<TestEntity>();
        var expected = Ray.Create(Fewple.NewPoint(-5, 0, -5), Fewple.NewVector(0, 0, 1));

        // Act
        ray.Intersect(entity);

        // Assert
        entity.SavedRay.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void EntityHasCorrectNormalAfterTranslation()
    {
        // Arrange
        var entity = CreateTestEntity()
            .Morph(Transform.Translation(0, 1, 0));
        var point = Fewple.NewPoint(0, 1.70711f, -0.70711f);
        var expected = Fewple.NewVector(0, 0.70711f, -0.70711f);

        // Act
        var actual = entity.NormalAt(point);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void EntityHasCorrectNormalAfterTransformation()
    {
        // Arrange
        var entity = CreateTestEntity()
            .Morph(Transform.RotationZ(float.Pi/5))
            .Morph(Transform.Scaling(1, 0.5f, 1));
        var point = Fewple.NewPoint(0, MathF.Sqrt(2)/2, -MathF.Sqrt(2)/2);
        var expected = Fewple.NewVector(0, 0.97014f, -0.24254f);

        // Act
        var actual = entity.NormalAt(point);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }


}
