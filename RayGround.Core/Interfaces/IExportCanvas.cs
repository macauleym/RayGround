namespace RayGround.Core.Interfaces;

public interface IExportCanvas
{
    string Extension { get; }
    
    Task<string> EncodeString(Canvas canvas);
}
