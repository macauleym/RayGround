using RayGround.Core;

namespace RayGround.Cli.Rendering.Blueprinting.Blueprints;

public class BlankBlueprint : IConstructable
{
    public Task<Blueprint> ConstructAsync(World into) =>
        Task.FromResult(Blueprint.Create("empty", into));
}
