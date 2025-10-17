using RayGround.Core.Extensions;
using RayGround.Core.Models;

namespace RayGround.Core.Operations;

public static class Illuminate
{
    public static RayColor Lighting(
          Material material
        , Light light
        , Fewple position
        , Fewple eyeVector
        , Fewple normalVector
        , bool inShadow
        ) {
        // Combine surface and light color/intensity.
        var effectiveColor = material.Color * light.Intensity;
        
        // Find direction to light source.
        var lightVec = (light.Position - position).Normalize();
        
        // Compute ambient contribution
        var ambient = effectiveColor * material.Ambient;

        if (inShadow)
            return ambient;
        
        // lightDotNormal represents the cosine of the angle
        // between the light vector and the normal vector. 
        // A negative number means the light is on the opposite
        // side of the surface, and gradually approach 1 when
        // they start to meet.
        RayColor diffuse;
        RayColor specular;
        var lightDotNormal = lightVec.Dot(normalVector);
        if (lightDotNormal < 0)
        {
            diffuse  = RayColor.Black;
            specular = RayColor.Black;
        }
        else
        {
            diffuse = effectiveColor * material.Diffuse * lightDotNormal;

            // reflectDotEye is the cosine of the angle between
            // the reflection and the eye. A negative number means 
            // the light reflects away from the eye.
            var reflectVec    = (-lightVec).Reflect(normalVector);
            var reflectDotEye = reflectVec.Dot(eyeVector);
            if (reflectDotEye <= 0)
                specular = RayColor.Create(0, 0, 0);
            else
            {
                var factor = MathF.Pow(reflectDotEye, material.Shininess);
                specular   = light.Intensity * material.Specular * factor;
            }
        }

        return ambient + diffuse + specular;
    }

    public static bool IsShadowed(Light light, Fewple point, List<Entity> entities)
    {
        var vector    = light.Position - point;
        var distance  = vector.Magnitude();
        var direction = vector.Normalize();

        var ray           = Ray.Create(point, direction);
        var intersections = ray.IntersectWorld(entities);

        var hit = intersections.Hit();

        return hit.HasValue
            && hit.Value.RayTime < distance;
    }
}
