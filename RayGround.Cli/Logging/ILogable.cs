namespace RayGround.Cli.Logging;

public interface ILogable
{
    void Log(string message);
    void LogSeparator();
}
