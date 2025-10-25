using RayGround.Core;
using RayGround.Core.Models;
using RayGround.Core.Models.Patterns;

namespace RayGround.Tests;

class TestPattern(Color primary, Color secondary) : Pattern(primary, secondary)
{
    public static TestPattern Create() =>
        new(Color.White, Color.Black);

    public override Color GetColor(Fewple at) =>
        Color.Create(at.X, at.Y, at.Z);
}
