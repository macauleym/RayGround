namespace RayGround.Cli.Logging;

public abstract class Logger : ILogable
{
    protected readonly string Separator;

    protected Logger(string separator)
    {
        Separator = separator;
    }
    
    public abstract void Log(string message);
    public abstract void LogSeparator();
}
