using System.Diagnostics;
using RayGround.Cli.Building;
using RayGround.Cli.Logging;
using RayGround.Cli.Rendering;
using RayGround.Core.Exporters;

const string SEPARATOR = "========================================";
var logger   = new ConsoleLogger(SEPARATOR);

var stopwatch = Stopwatch.StartNew();

var renderer = ChooseRenderer(Render.Patterns, logger);
await renderer.RenderAsync();

stopwatch.Stop();
logger.Log($"Processing finished!\n\t{stopwatch.ElapsedMilliseconds}ms.\n\t{GC.GetTotalMemory(forceFullCollection:true)}mb.");

return;

IRenderable ChooseRenderer(Render desired, ILogable logger)
{
    var brush    = new CanvasBrush();
    var exporter = new PPMExporter();

    var factory = new RendererFactory(brush, exporter, logger);

    return factory.Construct(desired);
}
