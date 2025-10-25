using RayGround.Core.Models.Patterns;

namespace RayGround.Core.Interfaces.Entities;

public interface IEtchable
{
    Material Etch(Pattern with);
}
