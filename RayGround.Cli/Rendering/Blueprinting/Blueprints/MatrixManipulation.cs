using RayGround.Cli.Logging;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Models;

namespace RayGround.Cli.Rendering.Blueprinting.Blueprints;

public class MatrixManipulation(ILogable logger) : IConstructable
{
    public Task<Blueprint> ConstructAsync(World into)
    {        
        logger.Log("Now for some matrix manipulation...");
        logger.LogSeparator();
    
        logger.Log("Taking the inverse of the identity matrix.");
        logger.Log(Matrix.Identity.Inverse().ToString());
        logger.LogSeparator();
    
        Console.WriteLine("Multiply matrix by its own inverse.");
        var m1 = new Matrix(4, 4)
            { [0] = [ 2 , 5 ,  7 , -4 ]
            , [1] = [ 9 , 6 , -8 ,  4 ]
            , [2] = [-2 , 1 ,  1 ,  3 ]
            , [3] = [ 4 , 3 ,  2 , -1 ]
            };
        var m2 = m1.Inverse();
        logger.Log(m1.ToString());
        logger.Log(m2.ToString());
        logger.Log((m1 * m2).ToString());
        logger.LogSeparator();

        logger.Log("Inverse Transpose and Transpose Inverse");
        logger.Log(m1.Inverse().Transpose().ToString());
        logger.Log(m1.Transpose().Inverse().ToString());
        logger.LogSeparator();
    
        logger.Log("Showing identity with tuple, but with single identity value changed.");
        var tup = Fewple.Create(4, 2, 1, 3);
        var strangeIdentity = new Matrix(4, 4)
            { [0] = [ 1 , 0 , 0 , 0 ]
            , [1] = [ 0 , 9 , 0 , 0 ]
            , [2] = [ 0 , 0 , 1 , 0 ]
            , [3] = [ 0 , 0 , 0 , 1 ]
            };
        logger.Log((Matrix.Identity * tup).ToString());
        logger.Log((strangeIdentity * tup).ToString());
        logger.LogSeparator();

        return Task.FromResult(Blueprint.Create(
              "matrix-log"
            , into
            ));
    }
}
