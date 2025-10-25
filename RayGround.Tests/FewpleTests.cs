using FluentAssertions;
using RayGround.Core.Extensions;
using RayGround.Core.Models;

namespace RayGround.Tests;

public class FewpleTests
{
    [Fact]
    public void IsPointWithW1()
    {
        // Arrange

        // Act
        var target = Fewple.Create(4.3f, -4.2f, 3.1f, 1.0f);
        
        // Assert
        target.W.Should().Be(1);
        target.W.Should().NotBe(0);
    }

    [Fact]
    public void IsVectorWithW0()
    {
        // Arrange

        // Act
        var target = Fewple.Create(4.3f, -4.2f, 3.1f, 0.0f);
        
        // Assert
        target.W.Should().Be(0);
        target.W.Should().NotBe(1);
    }
    
    [Fact]
    public void NewPointSetsWToOne()
    {
        // Arrange
        var expected = Fewple.Create(4f, -4f, 3f, 1f);

        // Act
        var target = Fewple.NewPoint(4f, -4f, 3f);
        
        // Assert
        target.Should().Be(expected);
    }

    [Fact]
    public void NewVectorSetsWToZero()
    {
        // Arrange
        var expected = Fewple.Create(4f, -4f, 3f, 0f);

        // Act
        var target = Fewple.NewVector(4f, -4f, 3f);
        
        // Assert
        target.Should().Be(expected);
    }

    [Fact]
    public void AddingPointAndVectorReturnsPoint()
    {
        // Arrange
        var point    = Fewple.Create(3, -2, 5, 1);
        var vector   = Fewple.Create(-2, 3, 1, 0);
        var expected = Fewple.NewPoint(1, 1, 6);
        
        // Act
        var actual = point + vector;
        
        // Assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void AddingTwoVectorsReturnsVector()
    {    
        // Arrange
        var point    = Fewple.Create(3, -2, 5, 0);
        var vector   = Fewple.Create(-2, 3, 1, 0);
        var expected = Fewple.NewVector(1, 1, 6);
    
        // Act
        var actual = point + vector;
    
        // Assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void AddingTwoPointsReturnsOther()
    {    
        // Arrange
        var point    = Fewple.Create(3, -2, 5, 1);
        var vector   = Fewple.Create(-2, 3, 1, 1);
        var expected = Fewple.Create(1, 1, 6, 2);
    
        // Act
        var actual = point + vector;
    
        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void SubtractingPointsReturnsVector()
    {
        // Arrange
        var p1       = Fewple.NewPoint(3, 2, 1);
        var p2       = Fewple.NewPoint(5, 6, 7);
        var expected = Fewple.NewVector(-2, -4, -6);

        // Act
        var actual = p1 - p2;

        // Assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void SubtractingVectorFromPointReturnsPoint()
    {
        // Arrange
        var p1       = Fewple.NewPoint(3, 2, 1);
        var p2       = Fewple.NewVector(5, 6, 7);
        var expected = Fewple.NewPoint(-2, -4, -6);

        // Act
        var actual = p1 - p2;

        // Assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void SubtractingVectorsReturnsVector()
    {
        // Arrange
        var p1       = Fewple.NewVector(3, 2, 1);
        var p2       = Fewple.NewVector(5, 6, 7);
        var expected = Fewple.NewVector(-2, -4, -6);

        // Act
        var actual = p1 - p2;

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void SubtractingZeroVectorReturnsNegated()
    {
        // Arrange
        var vector   = Fewple.NewVector(1, -2, 3);
        var expected = Fewple.NewVector(-1, 2, -3);

        // Act
        var actual = -vector;

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void MultiplyByNumberReturnsScaledUp()
    {
        // Arrange
        var tuple    = Fewple.Create(1, -2, 3, -4);
        var scaler   = 3.5f;
        var expected = Fewple.Create(3.5f, -7, 10.5f, -14);
        
        // Act
        var actual = tuple * scaler;
        
        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void MultiplyByFractionReturnsScaledDown()
    {
        // Arrange
        var tuple    = Fewple.Create(1, -2, 3, -4);
        var scaler   = 0.5f;
        var expected = Fewple.Create(0.5f, -1, 1.5f, -2);

        // Act
        var actual = tuple * scaler;

        // Assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void DivideByNumberReturnsScaledDown()
    {
        // Arrange
        var tuple    = Fewple.Create(1, -2, 3, -4);
        var scaler   = 2;
        var expected = Fewple.Create(0.5f, -1, 1.5f, -2);
        
        // Act
        var actual = tuple / scaler;
        
        // Assert
        actual.Should().Be(expected);
    }

    public static IEnumerable<object[]> MagnitudeTheories =>
        new List<object[]>
        { new object[] { Fewple.NewVector(1, 0, 0)    , 1f             }
        , new object[] { Fewple.NewVector(0, 1, 0)    , 1f             }
        , new object[] { Fewple.NewVector(0, 0, 1)    , 1f             }
        , new object[] { Fewple.NewVector(1, 2, 3)    , MathF.Sqrt(14) }
        , new object[] { Fewple.NewVector(-1, -2, -3) , MathF.Sqrt(14) }
        };
    
    [Theory]
    [MemberData(nameof(MagnitudeTheories))]
    public void ComputeMagnitudesReturnsFloat(Fewple vector, float expected)
    {
        // Arrange
        
        // Act
        var actual = vector.Magnitude();

        // Assert
        actual.Should().Be(expected);
    }

    public static IEnumerable<object[]> NormalizeTheories =>
        new List<object[]>
        { new object[] { Fewple.NewVector(4, 0, 0) , Fewple.NewVector(1,0,0) }
        , new object[] { Fewple.NewVector(1, 2, 3) , Fewple.NewVector(
                                                           1/MathF.Sqrt(14)
                                                         , 2/MathF.Sqrt(14)
                                                         , 3/MathF.Sqrt(14)
                                                         )}
        };
    
    [Theory]
    [MemberData(nameof(NormalizeTheories))]
    public void ComputeNormalForVectorReturnsVector(Fewple vector, Fewple expected)
    {
        // Arrange
        
        // Act
        var actual = vector.Normalize();

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void ComputeDotProductReturnsFloat()
    {
        // Arrange
        var a        = Fewple.NewVector(1, 2, 3);
        var b        = Fewple.NewVector(2, 3, 4);
        var expected = 20;

        // Act
        var actual = a.Dot(b);

        // Assert
        actual.Should().Be(expected);
    }

    public static IEnumerable<object[]> CrossProductTheories =>
        new List<object[]>
        { new object[] { Fewple.NewVector(1, 2, 3), Fewple.NewVector(2, 3, 4), Fewple.NewVector(-1, 2, -1) }
        , new object[] { Fewple.NewVector(2, 3, 4), Fewple.NewVector(1, 2, 3), Fewple.NewVector(1, -2, 1)  }
        };

    [Theory]
    [MemberData(nameof(CrossProductTheories))]
    public void ComputeCrossProductReturnsVector(Fewple first, Fewple second, Fewple expected)
    {
        // Arrange

        // Act
        var actual = first.Cross(second);

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void ReflectionInvertsYComponent()
    {
        // Arrange
        var vec      = Fewple.NewVector(1, -1, 0);
        var normal   = Fewple.NewVector(0, 1, 0);
        var expected = Fewple.NewVector(1, 1, 0);

        // Act
        var actual = vec.Reflect(normal);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CanReflectOffSlantedSurface()
    {
        // Arrange
        var vec      = Fewple.NewVector(0, -1, 0);
        var normal   = Fewple.NewVector(MathF.Sqrt(2) / 2, MathF.Sqrt(2) / 2, 0);
        var expected = Fewple.NewVector(1, 0, 0);

        // Act
        var actual = vec.Reflect(normal);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
