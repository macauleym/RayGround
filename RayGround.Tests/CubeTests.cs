using FluentAssertions;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Models.Entities;

namespace RayGround.Tests;

public class CubeTests
{
    public static IEnumerable<object[]> CubeIntersectionData = new List<object[]>
        { new object[] { "+x"     , Fewple.NewPoint( 5    , 0.5f ,  0 ) , Fewple.NewVector(-1 ,  0 ,  0) ,  4 , 6 }
        , new object[] { "-x"     , Fewple.NewPoint(-5    , 0.5f ,  0 ) , Fewple.NewVector( 1 ,  0 ,  0) ,  4 , 6 }
        , new object[] { "+y"     , Fewple.NewPoint( 0.5f ,   5  ,  0 ) , Fewple.NewVector( 0 , -1 ,  0) ,  4 , 6 }
        , new object[] { "-y"     , Fewple.NewPoint( 0.5f ,  -5  ,  0 ) , Fewple.NewVector( 0 ,  1 ,  0) ,  4 , 6 }
        , new object[] { "+z"     , Fewple.NewPoint( 0.5f ,   0  ,  5 ) , Fewple.NewVector( 0 ,  0 , -1) ,  4 , 6 }
        , new object[] { "-z"     , Fewple.NewPoint( 0.5f ,   0  , -5 ) , Fewple.NewVector( 0 ,  0 ,  1) ,  4 , 6 }
        , new object[] { "inside" , Fewple.NewPoint( 0    , 0.5f ,  0 ) , Fewple.NewVector( 0 ,  0 ,  1) , -1 , 1 }
        };
    
    [Theory]
    [MemberData(nameof(CubeIntersectionData))]
    public void RaysCanIntersectCubes(string name
    , Fewple origin
    , Fewple direction
    , float t1
    , float t2)
    {
        // Arrange
        var cube = Cube.Create();
        var ray  = Ray.Create(origin, direction);

        // Act
        var actual = cube.Intersections(ray);

        // Assert
        actual.Length.Should().Be(2);
        actual[0].Should().Be(t1);
        actual[1].Should().Be(t2);
    }
    
    public static IEnumerable<object[]> CubeMissedData = new List<object[]>
        { new object[] { "miss x"      , Fewple.NewPoint(-2 ,  0 ,  0 ) , Fewple.NewVector( 0.2673f , 0.5345f , 0.8018f ) }
        , new object[] { "miss y"      , Fewple.NewPoint( 0 , -2 ,  0 ) , Fewple.NewVector( 0.8018f , 0.2673f , 0.5345f ) }
        , new object[] { "miss z"      , Fewple.NewPoint( 0 ,  0 , -2 ) , Fewple.NewVector( 0.5345f , 0.8018f , 0.2673f ) }
        , new object[] { "opposite z " , Fewple.NewPoint( 2 ,  0 ,  2 ) , Fewple.NewVector( 0 ,  0 , -1) }
        , new object[] { "opposite y"  , Fewple.NewPoint( 0 ,  2 ,  2 ) , Fewple.NewVector( 0 , -1 ,  0) }
        , new object[] { "opposite x"  , Fewple.NewPoint( 2 ,  2 ,  0 ) , Fewple.NewVector(-1 ,  0 ,  0) }
        };
    
    [Theory]
    [MemberData(nameof(CubeMissedData))]
    public void RaysCanMissCubes(string name
    , Fewple origin
    , Fewple direction)
    {
        // Arrange
        var cube = Cube.Create();
        var ray  = Ray.Create(origin, direction);

        // Act
        var actual = cube.Intersections(ray);

        // Assert
        actual.Length.Should().Be(0);
    }
    
    public static IEnumerable<object[]> CubeNormalData = new List<object[]>
        { new object[] { "+x"      , Fewple.NewPoint( 1    ,  0.5f , -0.8f ) , Fewple.NewVector( 1 ,  0 ,  0 ) }
        , new object[] { "-x"      , Fewple.NewPoint(-1    , -0.2f ,  0.9f ) , Fewple.NewVector(-1 ,  0 ,  0 ) }
        , new object[] { "+y"      , Fewple.NewPoint(-0.4f ,    1  , -0.1f ) , Fewple.NewVector( 0 ,  1 ,  0 ) }
        , new object[] { "-y"      , Fewple.NewPoint( 0.3f ,   -1  , -0.7f ) , Fewple.NewVector( 0 , -1 ,  0 ) }
        , new object[] { "+z"      , Fewple.NewPoint(-0.6f ,  0.3f ,    1  ) , Fewple.NewVector( 0 ,  0 ,  1 ) }
        , new object[] { "-z"      , Fewple.NewPoint( 0.4f ,  0.4f ,   -1  ) , Fewple.NewVector( 0 ,  0 , -1 ) }
        , new object[] { "+corner" , Fewple.NewPoint( 1    ,    1  ,    1  ) , Fewple.NewVector( 1 ,  0 ,  0 ) }
        , new object[] { "-corner" , Fewple.NewPoint(-1    ,   -1  ,   -1  ) , Fewple.NewVector(-1 ,  0 ,  0 ) }
        };
    
    [Theory]
    [MemberData(nameof(CubeNormalData))]
    public void CanCalculateCubeNormal(string name
    , Fewple point
    , Fewple normal)
    {
        // Arrange
        var cube = Cube.Create();

        // Act
        var actual = cube.NormalAt(point);

        // Assert
        actual.Should().BeEquivalentTo(normal);
    }
}
