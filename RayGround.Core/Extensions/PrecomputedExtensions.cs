using RayGround.Core.Models;

namespace RayGround.Core.Extensions;

public static class PrecomputedExtensions
{
    public static float Schlick(this Precomputed source)
    {
        var cos = source.EyeVector.Dot(source.NormalVector);
        if (source.RefractionFrom > source.RefractionTo)
        {
            var refractionRatio = source.RefractionFrom / source.RefractionTo;
            var sin2T = MathF.Pow(refractionRatio, 2f) * (1f - MathF.Pow(cos, 2f));
            if (sin2T > 1f)
                return 1f;

            // Compute cosine of theta_t using trig identity.
            var cosT = MathF.Sqrt(1f - sin2T);
            
            // When refraction from is greater than refraction to, use cosine instead.
            cos = cosT;
        }

        var r0 = MathF.Pow((source.RefractionFrom - source.RefractionTo) 
                           / (source.RefractionFrom + source.RefractionTo)
                          , 2f);

        return r0 + (1f - r0) * MathF.Pow(1f - cos, 5f);
    }
}
