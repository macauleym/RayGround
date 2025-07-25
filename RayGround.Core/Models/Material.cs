namespace RayGround.Core;

public class Material
{
    public readonly float Ambient;
    public readonly float Diffuse;
    public readonly float Specular;
    public readonly float Shininess;
    public readonly RayColor Color;

    Material(float ambient, float diffuse, float specular, float shininess, RayColor color)
    {
        Ambient   = ambient;
        Diffuse   = diffuse;
        Specular  = specular;
        Shininess = shininess;
        Color     = color;
    }

    public static Material Create(
          float ambient = 0.1f
        , float diffuse = 0.9f
        , float specular = 0.9f
        , float shininess = 200.0f
        , RayColor? color = null
        ) => new(ambient, diffuse, specular, shininess, color ?? RayColor.Create(1, 1, 1));
}
