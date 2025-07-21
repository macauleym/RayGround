using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Operations;

namespace RayGround.Tests;

public class TransformTests
{
    [Fact]
    public void MultiplyingPointByTransformReturnsNewPoint()
    {
        // Arrange
        var transform = Transform.Translation(5, -3, 2);
        var point     = RayTuple.NewPoint(-3, 4, 5);
        var expected  = RayTuple.NewPoint(2, 1, 7);

        // Act
        var actual = transform * point;

        // Assert
        Assert.True(actual == expected);
    }

    [Fact]
    public void MultiplyingByInverseOfTranslationReturnsReverseMovement()
    {
        // Arrange
        var transform = Transform.Translation(5, -3, 2);
        var inverse   = transform.Inverse();
        var point     = RayTuple.NewPoint(-3, 4, 5);
        var expected  = RayTuple.NewPoint(-8, 7, 3);

        // Act
        var actual = inverse * point;
        
        // Assert
        Assert.True(actual == expected);
    }

    [Fact]
    public void TranslationDoesNotAffectVectors()
    {
        // Arrange
        var transform = Transform.Translation(5, -3, 2);
        var vector    = RayTuple.NewVector(-3, 4, 5);

        // Act
        var actual = transform * vector;

        // Assert
        Assert.True(actual == vector);
    }

    [Fact]
    public void MultiplyingPointByScaleReturnsDistantPoint()
    {
        // Arrange
        var transform = Transform.Scaling(2, 3, 4);
        var point     = RayTuple.NewPoint(-4, 6, 8);
        var expected  = RayTuple.NewPoint(-8, 18, 32);

        // Act
        var actual = transform * point;

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void MultiplyingVectorByScaleReturnsLargerVector()
    {
        // Arrange
        var transform = Transform.Scaling(2, 3, 4);
        var point     = RayTuple.NewVector(-4, 6, 8);
        var expected  = RayTuple.NewVector(-8, 18, 32);

        // Act
        var actual = transform * point;

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void MultiplyingByInverseScaleReduces()
    {
        // Arrange
        var transform = Transform.Scaling(2, 3, 4).Inverse();
        var point     = RayTuple.NewVector(-4, 6, 8);
        var expected  = RayTuple.NewVector(-2, 2, 2);

        // Act
        var actual = transform * point;

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void MultiplyingByNegativeScaleIsReflection()
    {
        // Arrange
        var transform = Transform.Scaling(-1, 1, 1);
        var point     = RayTuple.NewPoint(2, 3, 4);
        var expected  = RayTuple.NewPoint(-2, 3, 4);
        
        // Act
        var actual = transform * point;
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void MultiplyByRotationXMatrixReturnsRotatedAroundX()
    {
        // Arrange
        var halfQuarter  = Transform.RotationX(float.Pi / 4);
        var fullQuarter  = Transform.RotationX(float.Pi / 2);
        var point        = RayTuple.NewPoint(0, 1, 0);
        var expectedHalf = RayTuple.NewPoint(0, float.Sqrt(2) / 2, float.Sqrt(2) / 2);
        var expectedFull = RayTuple.NewPoint(0, 0, 1);

        // Act
        var actualHalf = halfQuarter * point;
        var actualFull = fullQuarter * point;

        // Assert
        actualHalf.Should().BeEquivalentTo(expectedHalf);
        actualFull.Should().BeEquivalentTo(expectedFull);
    }

    [Fact]
    public void MultiplyByInverseRotationIsReverseRotation()
    {
        // Arrange
        var halfQuarter = Transform.RotationX(float.Pi / 4);
        var inverseHalf = halfQuarter.Inverse();
        var point       = RayTuple.NewPoint(0, 1, 0);
        var expected    = RayTuple.NewPoint(0, float.Sqrt(2) / 2, -(float.Sqrt(2) / 2));
        
        // Act
        var actual = inverseHalf * point;
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void MultiplyByRotationYMatrixReturnsRotatedAroundY()
    {
        // Arrange
        var halfQuarter  = Transform.RotationY(float.Pi / 4);
        var fullQuarter  = Transform.RotationY(float.Pi / 2);
        var point        = RayTuple.NewPoint(0, 0, 1);
        var expectedHalf = RayTuple.NewPoint(float.Sqrt(2) / 2, 0, float.Sqrt(2) / 2);
        var expectedFull = RayTuple.NewPoint(1, 0, 0);

        // Act
        var actualHalf = halfQuarter * point;
        var actualFull = fullQuarter * point;

        // Assert
        actualHalf.Should().BeEquivalentTo(expectedHalf);
        actualFull.Should().BeEquivalentTo(expectedFull);
    }

    [Fact]
    public void MultiplyByRotationZMatrixReturnsRotatedAroundZ()
    {
        // Arrange
        var halfQuarter  = Transform.RotationZ(float.Pi / 4);
        var fullQuarter  = Transform.RotationZ(float.Pi / 2);
        var point        = RayTuple.NewPoint(0, 1, 0);
        var expectedHalf = RayTuple.NewPoint(-(float.Sqrt(2) / 2), float.Sqrt(2) / 2, 0);
        var expectedFull = RayTuple.NewPoint(-1, 0, 0);

        // Act
        var actualHalf = halfQuarter * point;
        var actualFull = fullQuarter * point;

        // Assert
        actualHalf.Should().BeEquivalentTo(expectedHalf);
        actualFull.Should().BeEquivalentTo(expectedFull);
    }

    public static IEnumerable<object[]> ShearingTheories =>
        new List<object[]>
        { new object[]
            { 1, 0, 0, 0, 0, 0
            , 5, 3, 4 
            }
        , new object[]
            { 0, 1, 0, 0, 0, 0
            , 6, 3, 4
            }
        , new object[]
            { 0, 0, 1, 0, 0, 0
            , 2, 5, 4
            }
        , new object[]
            { 0, 0, 0, 1, 0, 0
            , 2, 7, 4
            }
        , new object[]
            { 0, 0, 0, 0, 1, 0
            , 2, 3, 6
            }
        , new object[]
            { 0, 0, 0, 0, 0, 1
            , 2, 3, 7
            }
        };

    
    [Theory]
    [MemberData(nameof(ShearingTheories))]
    public void ShearingMovesAxisInProportionToOthers(
          float xy
        , float xz
        , float yx
        , float yz
        , float zx
        , float zy
        , float expectedX
        , float expectedY
        , float expectedZ
    ) {
        // Arrange
        var transform = Transform.Shearing(
              xy, xz
            , yx, yz
            , zx, zy
            );
        var point    = RayTuple.NewPoint(2, 3, 4);
        var expected = RayTuple.NewPoint(expectedX, expectedY, expectedZ);
        
        // Act
        var actual = transform * point;
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void IndividualTransformsAreAppliedInSequence()
    {
        // Arrange
        var point              = RayTuple.NewPoint(1, 0, 1);
        var rotateX            = Transform.RotationX(float.Pi / 2);
        var scale              = Transform.Scaling(5, 5, 5);
        var translate          = Transform.Translation(10, 5, 7);
        var expectedRotated    = RayTuple.NewPoint(1, -1, 0);
        var expectedScaled     = RayTuple.NewPoint(5, -5, 0);
        var expectedTranslated = RayTuple.NewPoint(15, 0, 7);

        // Act
        var actualRotated    = rotateX * point;
        var actualScaled     = scale * actualRotated;
        var actualTranslated = translate * actualScaled;

        // Assert
        actualRotated.Should().BeEquivalentTo(expectedRotated);
        actualScaled.Should().BeEquivalentTo(expectedScaled);
        actualTranslated.Should().BeEquivalentTo(expectedTranslated);
    }

    [Fact]
    public void ChainedTransformationsMustBeInReverseOrder()
    {
        // Arrange
        var point     = RayTuple.NewPoint(1, 0, 1);
        var rotateX   = Transform.RotationX(float.Pi / 2);
        var scale     = Transform.Scaling(5, 5, 5);
        var translate = Transform.Translation(10, 5, 7);
        var chained   = translate * scale * rotateX;
        var expected  = RayTuple.NewPoint(15, 0, 7);

        // Act
        var actual = chained * point;

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
