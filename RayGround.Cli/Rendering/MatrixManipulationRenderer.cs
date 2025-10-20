using RayGround.Cli.Logging;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Interfaces;
using RayGround.Core.Models;

namespace RayGround.Cli.Rendering;

public class MatrixManipulationRenderer(IExportCanvas exporter, ILogable logger) : Renderer(exporter, logger)
{
    async Task MatrixManipulationAsync()
    {
        Logger.Log("Now for some matrix manipulation...");
        Logger.LogSeparator();
    
        Logger.Log("Taking the inverse of the identity matrix.");
        Logger.Log(Matrix.Identity.Inverse().ToString());
        Logger.LogSeparator();
    
        Console.WriteLine("Multiply matrix by its own inverse.");
        var m1 = new Matrix(4, 4)
            { [0] = [ 2 , 5 ,  7 , -4 ]
            , [1] = [ 9 , 6 , -8 ,  4 ]
            , [2] = [-2 , 1 ,  1 ,  3 ]
            , [3] = [ 4 , 3 ,  2 , -1 ]
            };
        var m2 = m1.Inverse();
        Logger.Log(m1.ToString());
        Logger.Log(m2.ToString());
        Logger.Log((m1 * m2).ToString());
        Logger.LogSeparator();

        Logger.Log("Inverse Transpose and Transpose Inverse");
        Logger.Log(m1.Inverse().Transpose().ToString());
        Logger.Log(m1.Transpose().Inverse().ToString());
        Logger.LogSeparator();
    
        Logger.Log("Showing identity with tuple, but with single identity value changed.");
        var tup = Fewple.Create(4, 2, 1, 3);
        var strangeIdentity = new Matrix(4, 4)
            { [0] = [ 1 , 0 , 0 , 0 ]
            , [1] = [ 0 , 9 , 0 , 0 ]
            , [2] = [ 0 , 0 , 1 , 0 ]
            , [3] = [ 0 , 0 , 0 , 1 ]
            };
        Logger.Log((Matrix.Identity * tup).ToString());
        Logger.Log((strangeIdentity * tup).ToString());
        Logger.LogSeparator();
    }

    public override Task RenderAsync() =>
        MatrixManipulationAsync();
}
