using RayGround.Core;

namespace RayGround.Cli.Rendering.Capturing;

public class Capture
{
    public readonly string Name;
    public readonly Canvas Plate;

    Capture(string name, Canvas plate)
    {
        Name  = name;
        Plate = plate;
    }

    public static Capture Create(string name, Canvas plate) =>
        new(name, plate);
}
