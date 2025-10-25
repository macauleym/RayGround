using RayGround.Core.Interfaces.Entities;
using RayGround.Core.Models.Patterns;

namespace RayGround.Core;

public class Material
: IEtchable
{
    public float Ambient { get; private set; }
    public float Diffuse { get; private set; }
    public float Specular { get; private set; }
    public float Shininess { get; private set; }
    public float Reflective { get; private set; }
    public float Transparency { get; private set; }
    public float RefractionIndex { get; private set; }
    public Pattern Pattern { get; private set; }

    Material(
      float ambient
    , float diffuse
    , float specular
    , float shininess
    , float reflective
    , float transparency
    , float refractionIndex
    ) {
        Ambient         = ambient;
        Diffuse         = diffuse;
        Specular        = specular;
        Shininess       = shininess;
        Reflective      = reflective;
        Transparency    = transparency;
        RefractionIndex = refractionIndex;
        Pattern         = SolidPattern.Create(Color.White);
    }

    public static Material Create(
      float ambient         = 0.1f
    , float diffuse         = 0.9f
    , float specular        = 0.9f
    , float shininess       = 200.0f
    , float reflective      = 0f
    , float transparency    = 0f
    , float refractionIndex = 1.0f
    ) => new(ambient, diffuse, specular, shininess, reflective, transparency, refractionIndex);

    public Material Etch(Pattern with)
    {
        Pattern = with;

        return this;
    }
}
