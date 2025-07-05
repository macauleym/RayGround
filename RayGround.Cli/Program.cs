// See https://aka.ms/new-console-template for more information

using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Handlers;
using Environment = RayGround.Core.Environment;

var projectile = new Projectile(RayTuple.NewPoint(0,1,0), RayTuple.NewVector(1,1,0).Normalize());
var environment = new Environment(RayTuple.NewVector(0,-0.1f,0), RayTuple.NewVector(-0.01f,0,0));
var calculator = new EnvironmentCalculator();

Console.WriteLine($"Projectile starting at {projectile.Position}.");
Console.WriteLine("Fire!");
while (projectile.Position.Y > 0)
{
    projectile = calculator.Tick(projectile, environment);
    Console.WriteLine(projectile.Position);
}

Console.WriteLine("Landed!");