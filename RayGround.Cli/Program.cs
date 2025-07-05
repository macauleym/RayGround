using RayGround.Core;
using RayGround.Core.Exporters;
using RayGround.Core.Extensions;
using RayGround.Core.Handlers;
using RayEnvironment = RayGround.Core.Environment;

const string SEPARATOR = "========================================";

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
var ppm             = new PPMExporter();

Console.WriteLine(SEPARATOR);

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

Console.WriteLine(SEPARATOR);

Console.WriteLine("Exporting canvas to file...");
await File.WriteAllTextAsync("projectile.ppm", await ppm.ExportAsync(canvas));

Console.WriteLine(SEPARATOR);

Console.WriteLine("All done!");

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
