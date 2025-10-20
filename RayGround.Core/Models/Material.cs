using RayGround.Core.Models.Patterns;

namespace RayGround.Core;

public class Material
{
    public float Ambient { get; private set; }
    public float Diffuse { get; private set; }
    public float Specular { get; private set; }
    public float Shininess { get; private set; }
    public Color Color { get; private set; }
    public Pattern? Pattern { get; private set; }

    Material(float ambient, float diffuse, float specular, float shininess, Color color, Pattern? pattern = null)
    {
        Ambient   = ambient;
        Diffuse   = diffuse;
        Specular  = specular;
        Shininess = shininess;
        Color     = color;
        Pattern   = pattern;
    }

    public static Material Create(
      float ambient    = 0.1f
    , float diffuse    = 0.9f
    , float specular   = 0.9f
    , float shininess  = 200.0f
    , Color? color     = null
    , Pattern? pattern = null
    ) => new(ambient, diffuse, specular, shininess, color ?? Color.Create(1, 1, 1), pattern);

    public Material Recolor(Color with)
    {
        Color = with;

        return this;
    }

    public Material Etch(Pattern with)
    {
        Pattern = with;

        return this;
    }
}
