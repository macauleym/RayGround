using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Models.Patterns;
using RayGround.Core.Operations;

namespace RayGround.Tests;

public class PatternTests
{
    [Fact]
    public void CanCreateStripePattern()
    {
        // Arrange

        // Act
        var actual = Stripe.Default();

        // Assert
        actual.Primary.Should().BeEquivalentTo(Color.White);
        actual.Secondary.Should().BeEquivalentTo(Color.Black);
    }

    [Fact]
    public void StripePatternConstantInY()
    {
        // Arrange
        var pattern = Stripe.Default();

        // Act
        var actual = Enumerable.Range(0, 3)
            .Select(i => Fewple.NewPoint(0, i, 0))
            .Select(p => pattern.GetColor(p));

        // Assert
        actual.Should().AllBeEquivalentTo(Color.White);
    }

    [Fact]
    public void StripePatternConstantInZ()
    {
        // Arrange
        var pattern = Stripe.Default();

        // Act
        var actual = Enumerable.Range(0, 3)
            .Select(i => Fewple.NewPoint(0, 0, i))
            .Select(p => pattern.GetColor(p));

        // Assert
        actual.Should().AllBeEquivalentTo(Color.White);
    }

    [Fact]
    public void StripePatternAlternatesInX()
    {
        // Arrange
        var pattern = Stripe.Default();

        // Act
        var actual = new[] { 0, 0.9f, 1f, -0.1f, -1f, -1.1f }
            .Select(f => Fewple.NewPoint(f, 0, 0))
            .Select(pattern.GetColor)
            .ToArray();

        // Assert
        actual.Should().BeEquivalentTo([
          Color.White
        , Color.White
        , Color.Black
        , Color.Black
        , Color.Black
        , Color.White
        ]);
    }

    [Fact]
    public void StripesCanTransformWithEntity()
    {
        // Arrange
        var entity = Sphere
            .Create()
            .Morph(Transform.Scaling(2,2,2));
        var pattern = Stripe.Default();
        var worldPoint = Fewple.NewPoint(1.5f, 0, 0);
        
        // Act
        var actual = pattern.GetLocalizedColor(entity.Transform, worldPoint);

        // Assert
        actual.Should().BeEquivalentTo(Color.White);
    }

    [Fact]
    public void PatternDefaultTransformIsIdentity()
    {
        // Arrange
        var pattern = Stripe.Default();

        // Act
        var actual = pattern.Transform;

        // Assert
        actual.Should().BeEquivalentTo(Matrix.Identity);
    }

    [Fact]
    public void PatternCanBeMorphed()
    {
        // Arrange
        var pattern = Stripe.Default();

        // Act
        var actual = pattern.Morph(Transform.Translation(1, 2, 3));

        // Assert
        actual.Transform.Should().BeEquivalentTo(Transform.Translation(1, 2, 3));
    }

    [Fact]
    public void PatternCanBeBoundToLocalSpace()
    {
        // Arrange
        var entity = Sphere
            .Create()
            .Morph(Transform.Scaling(2, 2, 2));
        var pattern = TestPattern.Create();

        // Act
        var actual = pattern.GetLocalizedColor(entity.Transform, Fewple.NewPoint(2, 3, 4));

        // Assert
        actual.Should().BeEquivalentTo(Color.Create(1, 1.5f, 2));
    }

    [Fact]
    public void PatternOnEntityCanBeMorphed()
    {
        // Arrange
        var entity = Sphere
            .Create();
        var pattern = TestPattern
            .Create()
            .Morph(Transform.Scaling(2, 2, 2));

        // Act
        var actual = pattern.GetLocalizedColor(entity.Transform, Fewple.NewPoint(2, 3, 4));

        // Assert
        actual.Should().BeEquivalentTo(Color.Create(1, 1.5f, 2));
    }

    [Fact]
    public void PatternCanBeMorphedByItselfAndEntity()
    {
        // Arrange
        var entity = Sphere
            .Create()
            .Morph(Transform.Scaling(2, 2, 2));
        var pattern = TestPattern
            .Create()
            .Morph(Transform.Translation(0.5f, 1, 1.5f));

        // Act
        var actual = pattern.GetLocalizedColor(entity.Transform, Fewple.NewPoint(2.5f, 3, 3.5f));

        // Assert
        actual.Should().BeEquivalentTo(Color.Create(0.75f, 0.5f, 0.25f));
    }

    [Fact]
    public void GradientLinearlyInterpolatesBetweenColors()
    {
        // Arrange
        var pattern = Gradient.Default();

        // Act
        var actual = new[] { 0, 0.25f, 0.5f, 0.75f }
            .Select(x => Fewple.NewPoint(x, 0, 0))
            .Select(pattern.GetColor)
            .ToArray();

        // Assert
        actual.Should().BeEquivalentTo([
          Color.White
        , Color.Uniform(0.75f)
        , Color.Uniform(0.5f)
        , Color.Uniform(0.25f)
        ]);
    }

    [Fact]
    public void RingExtendsInBothXAndZ()
    {
        // Arrange
        var pattern = Rings.Default();

        // Act
        var actual = new[]
        { Fewple.NewPoint(0, 0, 0)
        , Fewple.NewPoint(1, 0, 0)
        , Fewple.NewPoint(0, 0, 1)
        , Fewple.NewPoint(0.708f, 0, 0.708f)
        }.Select(pattern.GetColor)
        .ToArray();

        // Assert
        actual[0].Should().BeEquivalentTo(Color.White);
        actual[1].Should().BeEquivalentTo(Color.Black);
        actual[2].Should().BeEquivalentTo(Color.Black);
        actual[3].Should().BeEquivalentTo(Color.Black);
    }

    public static IEnumerable<object[]> CheckerPatternAxisData = new List<object[]>
        { new object[] { "Repeat x-axis", new[] { Fewple.NewPoint(0, 0, 0), Fewple.NewPoint(0.99f, 0, 0), Fewple.NewPoint(1.01f, 0, 0) }}
        , new object[] { "Repeat y-axis", new[] { Fewple.NewPoint(0, 0, 0), Fewple.NewPoint(0, 0.99f, 0), Fewple.NewPoint(0, 1.01f, 0) }}
        , new object[] { "Repeat z-axis", new[] { Fewple.NewPoint(0, 0, 0), Fewple.NewPoint(0, 0, 0.99f), Fewple.NewPoint(0, 0, 1.01f) }}
        };
    
    [Theory]
    [MemberData(nameof(CheckerPatternAxisData))]
    public void CheckerPatternRepeatsAlongEachAxis(string testName, Fewple[] points)
    {
        // Arrange
        var pattern = Checker.Default();

        // Act
        var actual = points.Select(pattern.GetColor).ToArray();

        // Assert
        actual.Should().BeEquivalentTo([
          Color.White
        , Color.White
        , Color.Black
        ]);
    }
}
