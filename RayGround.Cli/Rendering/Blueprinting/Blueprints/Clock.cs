using RayGround.Cli.Logging;
using RayGround.Core;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Operations;

namespace RayGround.Cli.Rendering.Blueprinting.Blueprints;

public class Clock(ILogable logger) : IConstructable
{
    public Task<Blueprint> ConstructAsync(World into)
    {        
        var width  = 100f;
        var height = 100f;
    
        // Create the 12 points that we'll ultimately need to plot.
        var points = Enumerable.Range(0, 12)
            .Select(_ => Fewple.NewPoint(0, 1, 0))
            .ToArray();
    
        // Translation basis, setting the points around the center of the canvas.
        var centerBasis = Transform.Translation(40, 40, 0);
        var clockOffset = Transform.Scaling(width / 3, height / 3, 0);
    
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
            points[p] = rotate30r * points[p - 1];
            logger.Log($"Setting rotation point: {points[p]}");
        }
    
        // Then we scale and shift them to the "center" of the canvas.
        for (var p = 0; p < points.Length; p++)
        {
            points[p] = clockOffset * points[p];
            points[p] = centerBasis * points[p];
        
            logger.Log($"Setting position: {points[p]}");
        }
    
        // Paint the pixels onto the canvas.
        var spheres = points.Select(p => Sphere
            .Create()
            .Morph(Transform.Translation(p.X, p.Y, p.Z))
            .Morph(Transform.Scaling(0.1f, 0.1f, 0.1f))
            .Paint(Material
                .Create()
                .Bucket(Color.Create(225, 225, 0))));
        into.Lights.Add(Light.Create(Fewple.NewPoint(0,0,-10), Color.White));
        into.Entities.AddRange(spheres);
    
        logger.LogSeparator();
    
        // Return the canvas so we can export it, and see the pretty picture!
        return Task.FromResult(Blueprint.Create(
              "clock"
            , into
            ));
    }
}
