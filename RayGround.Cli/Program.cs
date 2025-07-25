using System.Diagnostics;
using System.Runtime.InteropServices;
using RayGround.Core;
using RayGround.Core.Calculators;
using RayGround.Core.Exporters;
using RayGround.Core.Extensions;
using RayGround.Core.Operations;
using RayEnvironment = RayGround.Core.Environment;

const string SEPARATOR = "========================================";

var stopwatch = Stopwatch.StartNew();

//var projectileCanvas = await LaunchProjectile();
//await ExportCanvas(projectileCanvas, "projectile.ppm");

//await MatrixManipulation();

//var clockCanvas = await HoursOnAClock();
//await ExportCanvas(clockCanvas, "clock.ppm");

//var raySphereCanvas = await RaysAtASphereAsync();
//await ExportCanvasAsync(raySphereCanvas, "rays-to-sphere.ppm");

var shadedSphereCanvas = await ShadedSphereAsync();
await ExportCanvasAsync(shadedSphereCanvas, "shaded-sphere.ppm");

stopwatch.Stop();
Console.WriteLine($"Processing finished!\n\t{stopwatch.ElapsedMilliseconds}ms.\n\t{GC.GetTotalMemory(forceFullCollection:true)}mb.");

return;

void Draw(RayCanvas canvas, RayTuple position, RayColor? color)
{
    var x = position.X;
    if (x > canvas.Width)
        x = canvas.Width;
    if (x < 0)
        x = 0;

    var y = position.Y;
    if (y > canvas.Height)
        y = canvas.Height;
    if (y < 0)
        y = 0;
    
    canvas.WritePixel((int)x, (int)canvas.Height - (int)y, color);
}

async Task ExportCanvasAsync(RayCanvas canvas, string fileName)
{
    var ppm = new PPMExporter();
    Console.WriteLine("Exporting canvas to file...");
    await File.WriteAllTextAsync(fileName, await ppm.ExportAsync(canvas));

    Console.WriteLine("All done!");
    Console.WriteLine(SEPARATOR);
}

async Task<RayCanvas> LaunchProjectileAsync()
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
    var projectileColor = RayColor.Create(0.8f, 0.8f, 0.8f);

    Console.WriteLine($"Projectile starting at {projectile.Position}.");
    Draw(canvas, projectile.Position, projectileColor);
    Console.WriteLine("Aim...");
    Console.WriteLine("Fire!");
    while (projectile.Position.Y > 0)
    {
        projectile = calculator.Tick(projectile, environment);
        Console.WriteLine(projectile.Position);
        Draw(canvas, projectile.Position, projectileColor);
    }

    Console.WriteLine("Landed!");
    Console.WriteLine(SEPARATOR);

    return canvas;
}

async Task MatrixManipulationAsync()
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
    Console.WriteLine(m1.Inverse().Transpose());
    Console.WriteLine(m1.Transpose().Inverse());
    Console.WriteLine(SEPARATOR);
    
    Console.WriteLine("Showing identity with tuple, but with single identity value changed.");
    var tup = RayTuple.Create(4, 2, 1, 3);
    var strangeIdentity = new RayMatrix(4, 4)
    { [0] = [ 1 , 0 , 0 , 0 ]
    , [1] = [ 0 , 9 , 0 , 0 ]
    , [2] = [ 0 , 0 , 1 , 0 ]
    , [3] = [ 0 , 0 , 0 , 1 ]
    };
    Console.WriteLine(RayMatrix.Identity * tup);
    Console.WriteLine(strangeIdentity * tup);
    Console.WriteLine(SEPARATOR);
}

async Task<RayCanvas> HoursOnAClockAsync()
{
    // First create the canvas that we'll be working with.
    var clock = new RayCanvas(100, 100);
    
    // Create the 12 points that we'll ultimately need to plot.
    var points = Enumerable.Range(0, 12)
        .Select(_ => RayTuple.NewPoint(0, 1, 0))
        .ToArray();
    
    // Translation basis, setting the points around the center of the canvas.
    var centerBasis = Transform.Translation(40, 40, 0);
    var clockOffset = Transform.Scaling(clock.Width/3, clock.Height/3, 0);
    
    // For the rotation, we need to rotate around the _Z_ axis.
    // Since we're looking at the x/y plane (down the Z axis)
    // rotating around either of those would make no sense.
    // We also want to rotate by 30degrees each rotation, which 
    // is equal to pi/6 radians.
    var rotate30r = Transform.RotationZ(float.Pi/6);
    
    // For each point, calculate it's position on the face, using the newly
    // implemented transformation tools.
    // We first rotate the points around the unit sphere.
    for (var p = 1; p < points.Length; p++)
    {
        points[p] = (rotate30r * points[p - 1]).ToTuple();
        Console.WriteLine($"Setting rotation point: {points[p]}");
    }
    
    // Then we scale and shift them to the "center" of the canvas.
    for (var p = 0; p < points.Length; p++)
    {
        points[p] = (clockOffset * points[p]).ToTuple();
        points[p] = (centerBasis * points[p]).ToTuple();
        
        Console.WriteLine($"Setting position: {points[p]}");
    }
    
    // Paint the pixels onto the canvas.
    foreach (var point in points)
        Draw(clock, point, RayColor.Create(225, 225, 0));
    
    Console.WriteLine(SEPARATOR);
    
    // Return the canvas so we can export it, and see the pretty picture!
    return clock;
}

async Task<RayCanvas> RaysAtASphereAsync()
{
    // Book recommends 100, but I want to flex.
    // Setting this to 500 took a bit over a minute (~72 seconds)
    // but like 30TB of total memory.
    // Need to look into better memory efficiency.
    var canvasSize = 500;
    var canvas     = new RayCanvas(canvasSize, canvasSize);
    var rayOrigin  = RayTuple.NewPoint(0, 0, -5);
    
    var sphere = Sphere.Create();
    
    // Transforming the sphere.
    var scaleY        = Transform.Scaling(1, .5f, 1);
    var sphereShrinkY = sphere.UpdateTransform(scaleY);
    
    var scaleX        = Transform.Scaling(.5f, 1, 1);
    var sphereShrinkX = sphere.UpdateTransform(scaleX);
    
    var shrinkAndRotate       = Transform.RotationZ(float.Pi / 4) * scaleX;
    var sphereShrinkAndRotate = sphere.UpdateTransform(shrinkAndRotate);
    
    var shrinkAndSkew       = Transform.Shearing(1, 0, 0, 0, 0, 0) * scaleX;
    var sphereShrinkAndSkew = sphere.UpdateTransform(shrinkAndSkew);
                    
    /********
     * Since the sphere is a unit sphere at the origin, tangent rays will
     * be approx. 1 unit from the origin. Every 5 units will increase the
     * height by another 1 unit. Since the wall is at 10, the ray will be
     * 3 units away from the origin. The wall must be double this (accounting
     * for halves) to ensure the sphere shadow will be printed on the
     * whole wall. This must be accounted for if changing the wall's, or
     * ray's position.
     ********/
    var wallZ     = 10;
    var wallSize  = 7f;
    
    var pixelSize = wallSize / canvasSize; // .07f
    var halfWall  = wallSize / 2;          // 3.5f

    for (var y = 0; y < canvasSize; y++)
    for (var x = 0; x < canvasSize; x++)
    {
        var worldY        = halfWall - pixelSize * y;
        var worldX        = -halfWall + pixelSize * x;
        var worldPosition = RayTuple.NewPoint(worldX, worldY, wallZ);

        var normalDir     = worldPosition - rayOrigin;
        var ray           = Ray.Create(rayOrigin, normalDir); 
        var intersections = ray.Intersect(sphere);
        
        if (intersections.Hit().HasValue)
            canvas.WritePixel(x, y, RayColor.Create(255, 0, 0));
    }

    return canvas;
}

async Task<RayCanvas> ShadedSphereAsync()
{
    // Book recommends 100, but I want to flex.
    // Setting this to 500 took a bit over a minute (~72 seconds)
    // but like 30TB of total memory.
    // Need to look into better memory efficiency.
    var canvasSize = 500;
    var canvas     = new RayCanvas(canvasSize, canvasSize);
    var rayOrigin  = RayTuple.NewPoint(0, 0, -5);

    var sphere = Sphere.Create()
        .UpdateMaterial(Material.Create(color: RayColor.Create(1, 0.2f, 1)));

    var light = Light.AsPoint(
          RayTuple.NewPoint(10, 5, -10)
        , RayColor.Create(1, 1, 1)
        );

    // Transforming the sphere.
    var scaleY        = Transform.Scaling(1, .5f, 1);
    var sphereShrinkY = sphere.UpdateTransform(scaleY);
    
    var scaleX        = Transform.Scaling(.5f, 1, 1);
    var sphereShrinkX = sphere.UpdateTransform(scaleX);
    
    var shrinkAndRotate       = Transform.RotationZ(float.Pi / 4) * scaleX;
    var sphereShrinkAndRotate = sphere.UpdateTransform(shrinkAndRotate);
    
    var shrinkAndSkew       = Transform.Shearing(1, 0, 0, 0, 0, 0) * scaleX;
    var sphereShrinkAndSkew = sphere.UpdateTransform(shrinkAndSkew);

    /********
     * Since the sphere is a unit sphere at the origin, tangent rays will
     * be approx. 1 unit from the origin. Every 5 units will increase the
     * height by another 1 unit. Since the wall is at 10, the ray will be
     * 3 units away from the origin. The wall must be double this (accounting
     * for halves) to ensure the sphere shadow will be printed on the
     * whole wall. This must be accounted for if changing the wall's, or
     * ray's position.
     ********/
    var wallZ    = 10;
    var wallSize = 7f;
    
    var pixelSize = wallSize / canvasSize; // .07f
    var halfWall  = wallSize / 2;          // 3.5f

    for (var y = 0; y < canvasSize; y++)
    for (var x = 0; x < canvasSize; x++)
    {
        var worldY        = halfWall - pixelSize * y;
        var worldX        = -halfWall + pixelSize * x;
        var worldPosition = RayTuple.NewPoint(worldX, worldY, wallZ);

        var normalDir     = worldPosition - rayOrigin;
        var ray           = Ray.Create(rayOrigin, normalDir.Normalize()); 
        var intersections = ray.Intersect(sphere);

        if (!intersections.Hit().HasValue)
            continue;
        
        var hit       = intersections.Hit()!.Value;
        var point     = ray.Position(hit.RayPoint);
        var hitSphere = (Sphere)hit.Collided;
        var normal    = hitSphere.NormalAt(point);
        var eye       = -ray.Direction;
        var color     = Illuminate.Lighting(hitSphere.Material, light, point, eye, normal);
            
        canvas.WritePixel(x, y, color);
    }

    return canvas;
}
