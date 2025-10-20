using RayGround.Core.Models.Patterns;

namespace RayGround.Core.Interfaces.Entities;

public interface IEtchable<out T>
{
    T Etch(Pattern with);
}
