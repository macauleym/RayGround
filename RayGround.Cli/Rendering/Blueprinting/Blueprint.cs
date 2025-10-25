using RayGround.Core;

namespace RayGround.Cli.Rendering.Blueprinting;

public class Blueprint
{
    public readonly string Name;
    public readonly World Imprint;

    Blueprint(string name, World imprint)
    {
        Name    = name;
        Imprint = imprint;
    }
    
    public static Blueprint Create(string name, World imprint) =>
        new(name, imprint);
}
