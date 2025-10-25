using System.Diagnostics;
using RayGround.Cli.Logging;
using RayGround.Cli.Rendering;
using RayGround.Cli.Rendering.Blueprinting;
using RayGround.Cli.Rendering.Capturing;
using RayGround.Cli.Rendering.Layering;
using RayGround.Cli.Rendering.Printing;
using RayGround.Core;
using RayGround.Core.Exporters;
using RayGround.Core.Extensions;
using RayGround.Core.Models;
using RayGround.Core.Operations;

const string SEPARATOR = "========================================";

var logger    = new ConsoleLogger(SEPARATOR);
var stopwatch = Stopwatch.StartNew();

// Ideally, this would be determined by input.
// That way, we can simply run whatever render we want from the command line.
var desiredRender = Render.Reflectfrac;

var frustum  = float.Pi / 3;
var position = View.Transform(
      Fewple.NewPoint( 0, 1.5f, -5)
    , Fewple.NewPoint( 0, 1   ,  0)
    , Fewple.NewVector(0, 1   ,  0)
    );

var camera800x400 = Camera.Create(800, 400, frustum);
var camera400x200 = Camera.Create(400, 200, frustum);
var camera        = camera800x400.Morph(position);

var world  = World.Empty();

var enclosure  = new CaptureEnclosure(camera, world, 50);
var ppmPrinter = new Printer(new PPMExporter(), logger);

var blueprint  = ChooseBlueprint(desiredRender, logger);
var layer      = ChooseLayer(desiredRender, logger);

var exposed  = await enclosure.CaptureAsync(blueprint);
var layered  = await layer.ApplyAsync(exposed);
var filename = await ppmPrinter.DevelopAsync(layered);

stopwatch.Stop();
logger.Log($"Processing finished!\nDeveloped image to file: {filename}\n\t{stopwatch.ElapsedMilliseconds}ms.\n\t{GC.GetTotalMemory(forceFullCollection:true)}mb.");

return;

IConstructable ChooseBlueprint(Render desired, ILogable logger)
{
    var factory = new BlueprintFactory(logger);

    return factory.Construct(desired);
}

ILayerable ChooseLayer(Render desired, ILogable logger)
{
    var factory = new LayerFactory(logger);

    return factory.Construct(desired);
}
