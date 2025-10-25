using RayGround.Core.Extensions;
using RayGround.Core.Interfaces;

namespace RayGround.Core.Models.Patterns;

/* TODO
 * Some extra items to think about implementing for this feature:
 * 1. Radial Gradient pattern.
 * 2. Nested patterns.
 * 3. Blended patterns.
 * 4. Perturbed patterns.
 */
public abstract class Pattern(Color primary, Color secondary)
: IMorphable<Pattern>
, ILocalizable
{
    public readonly Color Primary   = primary;
    public readonly Color Secondary = secondary;
    
    public Matrix Transform { get; private set; } = Matrix.Identity;

    public abstract Color GetColor(Fewple at);
    
    public Pattern Morph(Matrix with)
    {
        Transform = with * Transform;

        return this;
    }

    public Fewple Localize(Matrix with, Fewple at)
    {
        var localPoint   = with.Inverse() * at;
        var patternPoint = Transform.Inverse() * localPoint;

        return patternPoint;
    }
}
