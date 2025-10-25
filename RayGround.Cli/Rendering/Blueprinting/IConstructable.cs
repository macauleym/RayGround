using RayGround.Core;

namespace RayGround.Cli.Rendering.Blueprinting;

public interface IConstructable
{
    Task<Blueprint> ConstructAsync(World into);
}
