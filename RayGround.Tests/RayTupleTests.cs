using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Extensions;

namespace RayGround.Tests;

public class RayTupleTests
{
    [Fact]
    public void IsPointWithW1()
    {
        // Arrange

        // Act
        var target = new RayTuple(4.3f, -4.2f, 3.1f, 1.0f);
        
        // Assert
        target.W.Should().Be(1);
        target.W.Should().NotBe(0);
    }

    [Fact]
    public void IsVectorWithW0()
    {
        // Arrange

        // Act
        var target = new RayTuple(4.3f, -4.2f, 3.1f, 0.0f);
        
        // Assert
        target.W.Should().Be(0);
        target.W.Should().NotBe(1);
    }
    
    [Fact]
    public void NewPointSetsWTo1()
    {
        // Arrange
        var expected = new RayTuple(4f, -4f, 3f, 1f);

        // Act
        var target   = RayTuple.NewPoint(4f, -4f, 3f);
        
        // Assert
        target.Should().Be(expected);
    }

    [Fact]
    public void NewVectorSetsWTo0()
    {
        // Arrange
        var expected = new RayTuple(4f, -4f, 3f, 0f);

        // Act
        var target = RayTuple.NewVector(4f, -4f, 3f);
        
        // Assert
        target.Should().Be(expected);
    }

    [Fact]
    public void AddingPointAndVectorReturnsPoint()
    {
        // Arrange
        var point    = new RayTuple(3, -2, 5, 1);
        var vector   = new RayTuple(-2, 3, 1, 0);
        var expected = RayTuple.NewPoint(1, 1, 6);
        
        // Act
        var actual = point + vector;
        
        // Assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void AddingTwoVectorsReturnsVector()
    {    
        // Arrange
        var point    = new RayTuple(3, -2, 5, 0);
        var vector   = new RayTuple(-2, 3, 1, 0);
        var expected = RayTuple.NewVector(1, 1, 6);
    
        // Act
        var actual = point + vector;
    
        // Assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void AddingTwoPointsReturnsOther()
    {    
        // Arrange
        var point    = new RayTuple(3, -2, 5, 1);
        var vector   = new RayTuple(-2, 3, 1, 1);
        var expected = new RayTuple(1, 1, 6, 2);
    
        // Act
        var actual = point + vector;
    
        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void SubtractingPointsReturnsVector()
    {
        // Arrange
        var p1       = RayTuple.NewPoint(3, 2, 1);
        var p2       = RayTuple.NewPoint(5, 6, 7);
        var expected = RayTuple.NewVector(-2, -4, -6);

        // Act
        var actual = p1 - p2;

        // Assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void SubtractingVectorFromPointReturnsPoint()
    {
        // Arrange
        var p1       = RayTuple.NewPoint(3, 2, 1);
        var p2       = RayTuple.NewVector(5, 6, 7);
        var expected = RayTuple.NewPoint(-2, -4, -6);

        // Act
        var actual = p1 - p2;

        // Assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void SubtractingVectorsReturnsVector()
    {
        // Arrange
        var p1       = RayTuple.NewVector(3, 2, 1);
        var p2       = RayTuple.NewVector(5, 6, 7);
        var expected = RayTuple.NewVector(-2, -4, -6);

        // Act
        var actual = p1 - p2;

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void SubtractingZeroVectorReturnsNegated()
    {
        // Arrange
        var vector = RayTuple.NewVector(1, -2, 3);
        var expected = RayTuple.NewVector(-1, 2, -3);

        // Act
        var actual = -vector;

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void MultiplyByNumberReturnsScaledUp()
    {
        // Arrange
        var tuple = new RayTuple(1, -2, 3, -4);
        var scaler = 3.5f;
        var expected = new RayTuple(3.5f, -7, 10.5f, -14);
        
        // Act
        var actual = tuple * scaler;
        
        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void MultiplyByFractionReturnsScaledDown()
    {
        // Arrange
        var tuple = new RayTuple(1, -2, 3, -4);
        var scaler = 0.5f;
        var expected = new RayTuple(0.5f, -1, 1.5f, -2);

        // Act
        var actual = tuple * scaler;

        // Assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void DivideByNumberReturnsScaledDown()
    {
        // Arrange
        var tuple = new RayTuple(1, -2, 3, -4);
        var scaler = 2;
        var expected = new RayTuple(0.5f, -1, 1.5f, -2);
        
        // Act
        var actual = tuple / scaler;
        
        // Assert
        actual.Should().Be(expected);
    }

    public static IEnumerable<object[]> MagnitudeTheories =>
        new List<object[]>
        { new object[] { RayTuple.NewVector(1, 0, 0)    , 1f             }
        , new object[] { RayTuple.NewVector(0, 1, 0)    , 1f             }
        , new object[] { RayTuple.NewVector(0, 0, 1)    , 1f             }
        , new object[] { RayTuple.NewVector(1, 2, 3)    , MathF.Sqrt(14) }
        , new object[] { RayTuple.NewVector(-1, -2, -3) , MathF.Sqrt(14) }
        };
    
    [Theory]
    [MemberData(nameof(MagnitudeTheories))]
    public void ComputeMagnitudesReturnsFloat(RayTuple vector, float expected)
    {
        // Arrange
        
        // Act
        var actual = vector.Magnitude();

        // Assert
        actual.Should().Be(expected);
    }

    public static IEnumerable<object[]> NormalizeTheories =>
        new List<object[]>
        { new object[] { RayTuple.NewVector(4, 0, 0) , RayTuple.NewVector(1,0,0) }
        , new object[] { RayTuple.NewVector(1, 2, 3) , RayTuple.NewVector(
                                                           1/MathF.Sqrt(14)
                                                         , 2/MathF.Sqrt(14)
                                                         , 3/MathF.Sqrt(14)
                                                         )}
        };
    
    [Theory]
    [MemberData(nameof(NormalizeTheories))]
    public void ComputeNormalForVectorReturnsVector(RayTuple vector, RayTuple expected)
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
        var a = RayTuple.NewVector(1, 2, 3);
        var b = RayTuple.NewVector(2, 3, 4);
        var expected = 20;

        // Act
        var actual = a.Dot(b);

        // Assert
        actual.Should().Be(expected);
    }

    public static IEnumerable<object[]> CrossProductTheories =>
        new List<object[]>
        { new object[] { RayTuple.NewVector(1, 2, 3), RayTuple.NewVector(2, 3, 4), RayTuple.NewVector(-1, 2, -1) }
        , new object[] { RayTuple.NewVector(2, 3, 4), RayTuple.NewVector(1, 2, 3), RayTuple.NewVector(1, -2, 1)  }
        };

    [Theory]
    [MemberData(nameof(CrossProductTheories))]
    public void ComputeCrossProductReturnsVector(RayTuple first, RayTuple second, RayTuple expected)
    {
        // Arrange

        // Act
        var actual = first.Cross(second);

        // Assert
        actual.Should().Be(expected);
    }
}
