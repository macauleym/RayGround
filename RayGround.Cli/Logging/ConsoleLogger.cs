namespace RayGround.Cli.Logging;

public class ConsoleLogger(string separator) : Logger(separator)
{
    public override void Log(string message) =>
        Console.WriteLine(message);

    public override void LogSeparator() =>
        Console.WriteLine(Separator);
}
