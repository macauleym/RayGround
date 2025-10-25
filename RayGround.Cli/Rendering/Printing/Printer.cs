using RayGround.Cli.Logging;
using RayGround.Cli.Rendering.Capturing;
using RayGround.Core.Interfaces;

namespace RayGround.Cli.Rendering.Printing;

public class Printer(IExportCanvas exporter, ILogable logger) : IDevelopable
{
    public async Task<string> DevelopAsync(Capture capture)
    {
        logger.Log("Exporting canvas to file...");

        var encoded = await exporter.EncodeString(capture.Plate);
        var file    = capture.Name + "." + exporter.Extension;
        await File.WriteAllTextAsync(file, encoded);

        logger.Log("All done!");
        logger.LogSeparator();

        return file;
    }
}
