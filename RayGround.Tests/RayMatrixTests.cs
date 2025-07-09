using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Operations;

namespace RayGround.Tests;

public class RayMatrixTests
{
    [Fact]
    public void ConstructAndInspect()
    {
        // Arrange
        var expected00 = 1f;
        var expected03 = 4f;
        var expected10 = 5.5f;
        var expected12 = 7.5f;
        var expected22 = 11f;
        var expected30 = 13.5f;
        var expected32 = 15.5f;

        // Act
        var actual = new RayMatrix(4, 4)
        { [0] = [ 1f    , 2f    , 3f    , 4f    ]
        , [1] = [ 5.5f  , 6.5f  , 7.5f  , 8.5f  ]
        , [2] = [ 9f    , 10f   , 11f   , 12f   ]
        , [3] = [ 13.5f , 14.5f , 15.5f , 16.5f ]
        };

        // Assert
        actual[0, 0].Should().Be(expected00);
        actual[0, 3].Should().Be(expected03);
        actual[1, 0].Should().Be(expected10);
        actual[1, 2].Should().Be(expected12);
        actual[2, 2].Should().Be(expected22);
        actual[3, 0].Should().Be(expected30);
        actual[3, 2].Should().Be(expected32);
    }

    [Fact]
    public void RepresentTwoByTwo()
    {
        // Arrange
        var expected00 = -3f;
        var expected01 = 5f;
        var expected10 = 1f;
        var expected11 = -2;
        
        // Act
        var actual = new RayMatrix(2, 2)
        { [0] = [ -3 ,  5 ]
        , [1] = [  1 , -2 ]
        };

        // Assert
        actual[0, 0].Should().Be(expected00);
        actual[0, 1].Should().Be(expected01);
        actual[1, 0].Should().Be(expected10);
        actual[1, 1].Should().Be(expected11);
    }

    [Fact]
    public void RepresentThreeByThree()
    {
        // Arrange
        var expected00 = -3f;
        var expected11 = -2f;
        var expected22 = 1f;
        
        // Act
        var actual = new RayMatrix(3, 3)
        { [0] = [ -3 ,  5 ,  0 ]
        , [1] = [  1 , -2 , -7 ]
        , [2] = [  0 ,  1 ,  1 ]
        };

        // Assert
        actual[0, 0].Should().Be(expected00);
        actual[1, 1].Should().Be(expected11);
        actual[2, 2].Should().Be(expected22);
    }

    [Fact]
    public void IdenticalInstanceMatricesAreEqual()
    {
        // Arrange
        var m1 = new RayMatrix(4, 4)
        { [0] = [ 1 , 2 , 3 , 4 ]
        , [1] = [ 5 , 6 , 7 , 8 ]
        , [2] = [ 9 , 8 , 7 , 6 ]
        , [3] = [ 5 , 4 , 3 , 2 ]
        };
        var m2 = m1;
        
        // Act
        var actual = m1 == m2;
        
        // Act
        actual.Should().BeTrue();
    }
    
    [Fact]
    public void EqualValueMatricesAreEqual()
    {
        // Arrange
        var m1 = new RayMatrix(4, 4)
        { [0] = [ 1 , 2 , 3 , 4 ]
            , [1] = [ 5 , 6 , 7 , 8 ]
            , [2] = [ 9 , 8 , 7 , 6 ]
            , [3] = [ 5 , 4 , 3 , 2 ]
        };
        var m2 = new RayMatrix(4, 4)
        { [0] = [ 1 , 2 , 3 , 4 ]
            , [1] = [ 5 , 6 , 7 , 8 ]
            , [2] = [ 9 , 8 , 7 , 6 ]
            , [3] = [ 5 , 4 , 3 , 2 ]
        };
        
        // Act
        var actual = m1 == m2;
        
        // Act
        actual.Should().BeTrue();
    }
    
    [Fact]
    public void UnequalValueMatricesAreNotEqual()
    {
        // Arrange
        var m1 = new RayMatrix(4, 4)
        { [0] = [ 1 , 2 , 3 , 4 ]
        , [1] = [ 5 , 6 , 7 , 8 ]
        , [2] = [ 9 , 8 , 7 , 6 ]
        , [3] = [ 5 , 4 , 3 , 2 ]
        };
        var m2 = new RayMatrix(4, 4)
        { [0] = [ 2 , 3 , 4 , 5 ]
        , [1] = [ 6 , 7 , 8 , 9 ]
        , [2] = [ 8 , 7 , 6 , 5 ]
        , [3] = [ 4 , 3 , 2 , 1 ]
        };
        
        // Act
        var actual = m1 != m2;
        
        // Act
        actual.Should().BeTrue();
    }

    [Fact]
    public void MultiplyingByMatricesReturnsMatrixResult()
    {
        // Arrange
        var m1 = new RayMatrix(4, 4)
        { [0] = [ 1 , 2 , 3 , 4 ]
        , [1] = [ 5 , 6 , 7 , 8 ]
        , [2] = [ 9 , 8 , 7 , 6 ]
        , [3] = [ 5 , 4 , 3 , 2 ]
        };
        var m2 = new RayMatrix(4, 4)
        { [0] = [ -2 , 1 , 2 ,  3 ]
        , [1] = [  3 , 2 , 1 , -1 ]
        , [2] = [  4 , 3 , 6 ,  5 ]
        , [3] = [  1 , 2 , 7 ,  8 ]
        };
        var expected = new RayMatrix(4, 4)
        { [0] = [ 20 , 22 , 50  , 48  ]
        , [1] = [ 44 , 54 , 114 , 108 ]
        , [2] = [ 40 , 58 , 110 , 102 ]
        , [3] = [ 16 , 26 , 46  , 42  ]
        };
        
        // Act
        var actual = m1 * m2;

        // Assert
        Assert.True(actual == expected);
    }

    [Fact]
    public void MultiplyingByTupleReturnsOneColumnMatrix()
    {
        // Arrange
        var m1 = new RayMatrix(4, 4)
        { [0] = [ 1 , 2 , 3 , 4 ]
        , [1] = [ 2 , 4 , 4 , 2 ]
        , [2] = [ 8 , 6 , 4 , 1 ]
        , [3] = [ 0 , 0 , 0 , 1 ]
        };
        var t1 = new RayTuple(1, 2, 3, 1);
        var expected = new RayTuple(18, 24, 33, 1);

        // Act
        var actual = m1 * t1;

        // Assert
        actual.ToTuple().Should().Be(expected);
    }

    [Fact]
    public void MultiplyingByIdentityReturnsInputMatrix()
    {
        // Arrange
        var m1 = new RayMatrix(4, 4)
        { [0] = [ 0 , 1 ,  2 ,  4 ]
        , [1] = [ 1 , 2 ,  4 ,  8 ]
        , [2] = [ 2 , 4 ,  8 , 16 ]
        , [3] = [ 4 , 8 , 16 , 32 ]
        };
        var m2 = RayMatrix.Identity;
        var expected = m1;

        // Act
        var actual = m1 * m2;

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void TransposeMatrixRotatesAboutDiagonal()
    {
        // Arrange
        var m1 = new RayMatrix(4, 4)
        { [0] = [ 0 , 9 , 3 , 0 ]
        , [1] = [ 9 , 8 , 0 , 8 ]
        , [2] = [ 1 , 8 , 5 , 3 ]
        , [3] = [ 0 , 0 , 5 , 8 ]
        };
        var expected = new RayMatrix(4, 4)
        { [0] = [ 0 , 9 , 1 , 0 ]
        , [1] = [ 9 , 8 , 8 , 0 ]
        , [2] = [ 3 , 0 , 5 , 5 ]
        , [3] = [ 0 , 8 , 3 , 8 ]
        };
        
        // Act
        var actual = Calculate.Transpose(m1);
        
        // Act
        Assert.True(actual == expected);
    }
    
    [Fact]
    public void TransposeIdentityReturnsIdentity()
    {
        // Arrange
        
        // Act
        var actual = Calculate.Transpose(RayMatrix.Identity);
        
        // Act
        actual.Should().BeEquivalentTo(RayMatrix.Identity);
    }

    [Fact]
    public void DeterminantOfTwoByTwoReturnsValue()
    {
        // Arrange
        var m = new RayMatrix(2, 2)
        { [0] = [  1 , 5 ]
        , [1] = [ -3 , 2 ]
        };
        var expected = 17;

        // Act
        var actual = Calculate.Determinant(m);

        // Assert
        actual.Should().Be(expected);
    }

    public static IEnumerable<object[]> SubmatrixTheories =>
        new List<object[]>
        { new object[] { 
                0, 2,
                new RayMatrix(3, 3)
                { [0] = [  1 , 5 ,  0 ]
                , [1] = [ -3 , 2 ,  7 ]
                , [2] = [  0 , 6 , -3 ]
                }
                , new RayMatrix(2, 2)
                { [0] = [ -3 , 2 ]
                , [1] = [  0 , 6 ]
                }
            }
        , new object[] {
                2, 1, 
                new RayMatrix(4, 4)
                { [0] = [ -6 , 1 ,  1 , 6 ]
                , [1] = [ -8 , 5 ,  8 , 6 ]
                , [2] = [ -1 , 0 ,  8 , 2 ]
                , [3] = [ -7 , 1 , -1 , 1 ]
                }
                , new RayMatrix(3,3)
                { [0] = [ -6 ,  1 , 6 ]
                , [1] = [ -8 ,  8 , 6 ]
                , [2] = [ -7 , -1 , 1 ]
                }
            }
        };
    
    [Theory]
    [MemberData(nameof(SubmatrixTheories))]
    public void CreatesSubmatrixWithoutGivenRowAndColumn(int row, int column, RayMatrix m, RayMatrix expected)
    {
        // Arrange
        
        // Act
        var actual = Calculate.Submatrix(m, row, column);
        
        // Assert
        Assert.True(actual == expected);
    }

    public static IEnumerable<object[]> MinorTheories =>
        new List<object[]>
        { new object[] { 
              1
            , 0
            , 25
            }
        , new object[] {
              1
            , 2
            , -33
            }
        };
    
    [Theory]
    [MemberData(nameof(MinorTheories))]
    public void CalculatesMinorOfMatrixAtElement(int row, int column, int expected)
    {
        // Arrange
        var m = new RayMatrix(3, 3)
        { [0] = [ 3 ,  5 ,  0 ]
        , [1] = [ 2 , -1 , -7 ]
        , [2] = [ 6 , -1 ,  5 ]
        };
        var sub = Calculate.Submatrix(m, row, column);

        // Act
        var actual = Calculate.Minor(m, row, column);
        var subDet = Calculate.Determinant(sub);

        // Assert
        actual.Should().Be(subDet);
        actual.Should().Be(expected);
    }
    
    public static IEnumerable<object[]> CofactorTheories =>
        new List<object[]>
        { new object[] { 
              0
            , 0
            , -12
            }
        , new object[] {
              1
            , 0
            , -25
            }
        };
    
    [Theory]
    [MemberData(nameof(CofactorTheories))]
    public void CalculatesCofactorOfMatrixAtElement(int row, int column, int expected)
    {
        // Arrange
        var m = new RayMatrix(3, 3)
        { [0] = [ 3 ,  5 ,  0 ]
        , [1] = [ 2 , -1 , -7 ]
        , [2] = [ 6 , -1 ,  5 ]
        };

        // Act
        var actual = Calculate.Cofactor(m, row, column);

        // Assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void CalculatesCofactorOfLargerThreeByThreeMatrix()
    {
        // Arrange
        var m = new RayMatrix(3, 3)
        { [0] = [ 1  ,  2 ,  6 ]
        , [1] = [ -5 ,  8 , -4 ]
        , [2] = [  2 ,  6 ,  4 ]
        };
        var expectedCofactor1 = 56;
        var expectedCofactor2 = 12;
        var expectedCofactor3 = -46;
        var expected          = -196;
        
        // Act
        var cofactor1 = Calculate.Cofactor(m, 0, 0);
        var cofactor2 = Calculate.Cofactor(m, 0, 1);
        var cofactor3 = Calculate.Cofactor(m, 0, 2);
        var actual    = Calculate.Determinant(m);

        // Assert
        cofactor1.Should().Be(expectedCofactor1);
        cofactor2.Should().Be(expectedCofactor2);
        cofactor3.Should().Be(expectedCofactor3);
           actual.Should().Be(expected);
    }
    
    [Fact]
    public void CalculatesCofactorOfLargerFourByFour()
    {
        // Arrange
        var m = new RayMatrix(4, 4)
        { [0] = [ -2 , -8 ,  3 ,  5 ]
        , [1] = [ -3 ,  1 ,  7 ,  3 ]
        , [2] = [  1 ,  2 , -9 ,  6 ]
        , [3] = [ -6 ,  7 ,  7 , -9 ]
        };
        var expectedCofactor1 = 690;
        var expectedCofactor2 = 447;
        var expectedCofactor3 = 210;
        var expectedCofactor4 = 51;
        var expected          = -4071;
        
        // Act
        var cofactor1 = Calculate.Cofactor(m, 0, 0);
        var cofactor2 = Calculate.Cofactor(m, 0, 1);
        var cofactor3 = Calculate.Cofactor(m, 0, 2);
        var cofactor4 = Calculate.Cofactor(m, 0, 3);
        var actual    = Calculate.Determinant(m);

        // Assert
        cofactor1.Should().Be(expectedCofactor1);
        cofactor2.Should().Be(expectedCofactor2);
        cofactor3.Should().Be(expectedCofactor3);
        cofactor4.Should().Be(expectedCofactor4);
        actual.Should().Be(expected);
    }

    [Fact]
    public void ValidateMatrixIsInvertible()
    {
        // Arrange
        var m = new RayMatrix(4, 4)
        { [0] = [ 6 ,  4 , 4 ,  4 ]
        , [1] = [ 5 ,  5 , 7 ,  6 ]
        , [2] = [ 4 , -9 , 3 , -7 ] 
        , [3] = [ 9 ,  1 , 7 , -6 ]
        };
        var expected = -2120;

        // Act
        var det    = Calculate.Determinant(m);
        var actual = Calculate.IsInvertible(m);

        // Assert
        det.Should().NotBe(0);
        det.Should().Be(expected);
        actual.Should().BeTrue();
    }

    [Fact]
    public void ValidateMatrixIsNoninvertible()
    {
        // Arrange
        var m = new RayMatrix(4, 4)
        { [0] = [ -4 ,  2 , -2 , -3 ]
        , [1] = [  9 ,  6 ,  2 ,  6 ]
        , [2] = [  0 , -5 ,  1 , -5 ] 
        , [3] = [  0 ,  0 ,  0 ,  0 ]
        };

        // Act
        var det    = Calculate.Determinant(m);
        var actual = Calculate.IsInvertible(m);
        
        // Assert
        det.Should().Be(0);
        actual.Should().BeFalse();
    }

    public static IEnumerable<object[]> InverseTheories =>
        new List<object[]>
        { new object[] { 
                new RayMatrix(4, 4)
                { [0] = [ -5 ,  2 ,  6 , -8 ]
                , [1] = [  1 , -5 ,  1 ,  8 ]
                , [2] = [  7 ,  7 , -6 , -7 ] 
                , [3] = [  1 , -3 ,  7 ,  4 ]
                }
                , new RayMatrix(4, 4)
                { [0] = [  0.21805f ,  0.45113f ,  0.24060f , -0.04511f ]
                , [1] = [ -0.80827f , -1.45677f , -0.44361f ,  0.52068f ]
                , [2] = [ -0.07895f , -0.22368f , -0.05263f ,  0.19737f ]
                , [3] = [ -0.52256f , -0.81391f , -0.30075f ,  0.30639f ]
                }
            }
        , new object[] { 
                new RayMatrix(4, 4)
                { [0] = [  8 , -5 ,  9 ,  2 ]
                , [1] = [  7 ,  5 ,  6 ,  1 ]
                , [2] = [ -6 ,  0 ,  9 ,  6 ] 
                , [3] = [ -3 ,  0 , -9 , -4 ]
                }
                , new RayMatrix(4, 4)
                { [0] = [ -0.15385f , -0.15385f , -0.28205f , -0.53846f ]
                , [1] = [ -0.07692f ,  0.12308f ,  0.02564f ,  0.03077f ]
                , [2] = [  0.35897f ,  0.35897f ,  0.43590f ,  0.92308f ]
                , [3] = [ -0.69231f , -0.69231f , -0.76923f , -1.92308f ]
                }
            }
        , new object[] { 
                new RayMatrix(4, 4)
                { [0] = [  9 ,  3 ,  0 ,  9 ]
                , [1] = [ -5 , -2 , -6 , -3 ]
                , [2] = [ -4 ,  9 ,  6 ,  4 ] 
                , [3] = [ -7 ,  6 ,  6 ,  2 ]
                }
                , new RayMatrix(4, 4)
                { [0] = [ -0.04074f , -0.07778f ,  0.14444f , -0.22222f ]
                , [1] = [ -0.07778f ,  0.03333f ,  0.36667f , -0.33333f ]
                , [2] = [ -0.02901f , -0.14630f , -0.10926f ,  0.12963f ]
                , [3] = [  0.17778f ,  0.06667f , -0.26667f ,  0.33333f ]
                }
            }
        };
    
    [Theory]
    [MemberData(nameof(InverseTheories))]
    public void CalculateTheInverseOfAMatrix(RayMatrix matrix, RayMatrix expected)
    {
        // Arrange

        // Act
        var cofactor1 = Calculate.Cofactor(matrix, 2, 3);
        var cofactor2 = Calculate.Cofactor(matrix, 3, 2);
        var det       = Calculate.Determinant(matrix);
        var actual    = Calculate.Inverse(matrix);

        // Assert
        expected[3, 2].Should().Be(MathF.Round(cofactor1 / det, 5));
        expected[2, 3].Should().Be(MathF.Round(cofactor2 / det, 5));
        Assert.True(actual == expected);
    }

    [Fact]
    public void UndoMultiplyThroughInverseProduct()
    {
        // Arrange
        var m1 = new RayMatrix(4, 4)
        { [0] = [  3 , -9 ,  7 ,  3 ]
        , [1] = [  3 , -8 ,  2 , -9 ]
        , [2] = [ -4 ,  4 ,  4 ,  1 ]
        , [3] = [ -6 ,  5 , -1 ,  1 ]
        };
        var m2 = new RayMatrix(4, 4)
        { [0] = [  8 ,  2 ,  2 ,  2 ]
        , [1] = [  3 , -1 ,  7 ,  0 ]
        , [2] = [  7 ,  0 ,  5 ,  4 ]
        , [3] = [  6 , -2 ,  0 ,  5 ]
        };
        var m3 = m1 * m2;

        // Act
        var actual = m3 * Calculate.Inverse(m2);

        // Assert
        Assert.True(actual == m1);
    }
}
