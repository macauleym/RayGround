using RayGround.Cli.Logging;
using RayGround.Core;
using RayGround.Core.Interfaces;

namespace RayGround.Cli.Rendering;

public abstract class Renderer(IExportCanvas exporter, ILogable logger) : IRenderable
{
    protected readonly IExportCanvas Exporter = exporter;
    protected readonly ILogable Logger        = logger;

    protected async Task ExportCanvasAsync(Canvas canvas, string fileName)
    {
        Logger.Log("Exporting canvas to file...");
        await File.WriteAllTextAsync(fileName, await Exporter.ExportAsync(canvas));

        Logger.Log("All done!");
        Logger.LogSeparator();
    }
    
    public abstract Task RenderAsync();
}
