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
public abstract class Pattern
: IMorphable<Pattern>
, ILocalizable
{
    public readonly Color Primary;
    public readonly Color Secondary;
    
    public Matrix Transform { get; protected set; }
    
    protected Pattern(Color primary, Color secondary, Matrix? transform)
    {
        Primary   = primary;
        Secondary = secondary;
        Transform = transform ?? Matrix.Identity;
    }
    
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
