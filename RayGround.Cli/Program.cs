using RayGround.Core;
using RayGround.Core.Exporters;
using RayGround.Core.Extensions;
using RayGround.Core.Handlers;
using RayEnvironment = RayGround.Core.Environment;

const string SEPARATOR = "========================================";

//var projectileCanvas = await LaunchProjectile();
//Console.WriteLine(SEPARATOR);

//await ExportCanvas(projectileCanvas);
//Console.WriteLine(SEPARATOR);

await MatrixManipulation();
Console.WriteLine(SEPARATOR);

return;

void Draw(RayCanvas c, Projectile p, RayColor color)
{
    var x = p.Position.X;
    if (x > c.Width)
        x = c.Width;
    if (x < 0)
        x = 0;

    var y = p.Position.Y;
    if (y > c.Height)
        y = c.Height;
    if (y < 0)
        y = 0;
    
    c.WritePixel((int)x, (int)c.Height - (int)y, color);
}

async Task<RayCanvas> LaunchProjectile()
{
    Console.WriteLine("Prep the projectile...");
    var start      = RayTuple.NewPoint(0, 1, 0);
    var velocity   = RayTuple.NewVector(1, 1.8f, 0).Normalize() * 11.25f;
    var projectile = new Projectile(start, velocity);

    Console.WriteLine("Get the environment...");
    var gravity     = RayTuple.NewVector(0, -0.1f, 0);
    var wind        = RayTuple.NewVector(-0.01f, 0, 0);
    var environment = new RayEnvironment(gravity, wind);
    var calculator  = new EnvironmentCalculator();

    Console.WriteLine("Build the canvas...");
    var canvas          = new RayCanvas(900, 550);
    var projectileColor = new RayColor(0.8f, 0.8f, 0.8f);

    Console.WriteLine($"Projectile starting at {projectile.Position}.");
    Draw(canvas, projectile, projectileColor);
    Console.WriteLine("Aim...");
    Console.WriteLine("Fire!");
    while (projectile.Position.Y > 0)
    {
        projectile = calculator.Tick(projectile, environment);
        Console.WriteLine(projectile.Position);
        Draw(canvas, projectile, projectileColor);
    }

    Console.WriteLine("Landed!");

    return canvas;
}

async Task ExportCanvas(RayCanvas canvas)
{
    var ppm = new PPMExporter();
    Console.WriteLine("Exporting canvas to file...");
    await File.WriteAllTextAsync("projectile.ppm", await ppm.ExportAsync(canvas));

    Console.WriteLine("All done!");
}

async Task MatrixManipulation()
{
    Console.WriteLine("Now for some matrix manipulation...");
    Console.WriteLine(SEPARATOR);
    
    Console.WriteLine("Taking the inverse of the identity matrix.");
    Console.WriteLine(RayMatrix.Identity.Inverse());
    Console.WriteLine(SEPARATOR);
    
    Console.WriteLine("Multiply matrix by its own inverse.");
    var m1 = new RayMatrix(4, 4)
    { [0] = [ 2 , 5 ,  7 , -4 ]
    , [1] = [ 9 , 6 , -8 ,  4 ]
    , [2] = [-2 , 1 ,  1 ,  3 ]
    , [3] = [ 4 , 3 ,  2 , -1 ]
    };
    var m2 = m1.Inverse();
    Console.WriteLine(m1);
    Console.WriteLine(m2);
    Console.WriteLine(m1 * m2);
    Console.WriteLine(SEPARATOR);

    Console.WriteLine("Inverse Transpose and Transpose Inverse");
    Console.WriteLine(m1.Transpose().Inverse());
    Console.WriteLine(m1.Inverse().Transpose());
    Console.WriteLine(SEPARATOR);
    
    Console.WriteLine("Showing identity with tuple, but with single identity value changed.");
    var tup = new RayTuple(4, 2, 1, 3);
    var strangeIdentity = new RayMatrix(4, 4)
    { [0] = [ 1 , 0 , 0 , 0 ]
    , [1] = [ 0 , 9 , 0 , 0 ]
    , [2] = [ 0 , 0 , 1 , 0 ]
    , [3] = [ 0 , 0 , 0 , 1 ]
    };
    Console.WriteLine(RayMatrix.Identity * tup.ToMatrix());
    Console.WriteLine(strangeIdentity * tup.ToMatrix());
    Console.WriteLine(SEPARATOR);
}
