namespace RayGround.Core.Interfaces;

public interface IExportCanvas
{
    Task<string> ExportAsync(RayCanvas canvas);
}
