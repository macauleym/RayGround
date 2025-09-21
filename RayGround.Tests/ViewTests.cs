using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Operations;

namespace RayGround.Tests;

public class ViewTests
{
    [Fact]
    public void ViewTransformDefaultIsIdentityMatrix()
    {
        // Arrange
        var from = RayTuple.NewPoint(0, 0, 0);
        var to = RayTuple.NewPoint(0, 0, -1);
        var up = RayTuple.NewVector(0, 1, 0);
        var expected = RayMatrix.Identity;
        
        // Act
        var actual = View.Transform(from, to, up);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ViewTransformCanLookInPositiveZDirection()
    {
        // Arrange
        var from = RayTuple.NewPoint(0, 0, 0);
        var to = RayTuple.NewPoint(0, 0, 1);
        var up = RayTuple.NewVector(0, 1, 0);
        var expected = Transform.Scaling(-1, 1, -1);
        
        // Act
        var actual = View.Transform(from, to, up);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ViewTransformMovesTheWorld()
    {
        // Arrange
        var from = RayTuple.NewPoint(0, 0, 0);
        var to = RayTuple.NewPoint(0, 0, 0);
        var up = RayTuple.NewVector(0, 1, 0);
        var expected = Transform.Translation(0, 0, -8);
        
        // Act
        var actual = View.Transform(from, to, up);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ViewTransformCanHaveArbitraryInputs()
    {
        // Arrange
        var from = RayTuple.NewPoint(1, 3, 2);
        var to = RayTuple.NewPoint(4, -2, 8);
        var up = RayTuple.NewVector(1, 1, 0);
        var expected = new RayMatrix(4, 4)
        { [0] = [ -0.50709f , 0.50709f ,  0.67612f , -2.36643f ]
        , [1] = [  0.76772f , 0.60609f ,  0.12122f , -2.82843f ]
        , [2] = [ -0.35857f , 0.59761f , -0.71714f ,  0.00000f ]
        , [3] = [  0.00000f , 0.00000f ,  0.00000f ,  1.00000f ]
        };
        
        // Act
        var actual = View.Transform(from, to, up);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}