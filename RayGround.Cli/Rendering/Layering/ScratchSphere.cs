using RayGround.Cli.Rendering.Capturing;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Operations;

namespace RayGround.Cli.Rendering.Layering;

public class ScratchSphere : Layer
{
    public override Task<Capture> ApplyAsync(Capture source)
    {        
        // Book recommends 100, but I want to flex.
        // Setting this to 500 took a bit over a minute (~72 seconds)
        // but like 30TB of total memory.
        // Need to look into better memory efficiency.
        var canvasSize = 500;
        var canvas     = new Canvas(canvasSize, canvasSize);
        var rayOrigin  = Fewple.NewPoint(0, 0, -5);
        
        var sphere = Sphere.Create();
        
        // Transforming the sphere.
        var scaleY        = Transform.Scaling(1, .5f, 1);
        var sphereShrinkY = sphere.Morph(scaleY);
        
        var scaleX        = Transform.Scaling(.5f, 1, 1);
        var sphereShrinkX = sphere.Morph(scaleX);
        
        var shrinkAndRotate       = Transform.RotationZ(float.Pi / 4) * scaleX;
        var sphereShrinkAndRotate = sphere.Morph(shrinkAndRotate);
        
        var shrinkAndSkew       = Transform.Shearing(1, 0, 0, 0, 0, 0) * scaleX;
        var sphereShrinkAndSkew = sphere.Morph(shrinkAndSkew);
                        
        /********
         * Since the sphere is a unit sphere at the origin, tangent rays will
         * be approx. 1 unit from the origin. Every 5 units will increase the
         * height by another 1 unit. Since the wall is at 10, the ray will be
         * 3 units away from the origin. The wall must be float this (accounting
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
            var worldPosition = Fewple.NewPoint(worldX, worldY, wallZ);

            var normalDir     = worldPosition - rayOrigin;
            var ray           = Ray.Create(rayOrigin, normalDir); 
            var intersections = ray.Intersect(sphere);
            
            if (intersections.Hit()  is not null)
                canvas.WritePixel(x, y, Color.Create(255, 0, 0));
        }

        return Task.FromResult(Capture.Create(source.Name, canvas));
    }
}
