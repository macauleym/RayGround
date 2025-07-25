namespace RayGround.Core.Operations;

public static class Transform
{
    public static RayMatrix Translation(float x, float y, float z)
    {
        var translationMatrix   = RayMatrix.Identity.CloneMatrix();
        translationMatrix[0, 3] = x;
        translationMatrix[1, 3] = y;
        translationMatrix[2, 3] = z;

        return translationMatrix;
    }

    public static RayMatrix Scaling(float x, float y, float z)
    {        
        var scaleMatrix   = RayMatrix.Identity.CloneMatrix();
        scaleMatrix[0, 0] = x;
        scaleMatrix[1, 1] = y;
        scaleMatrix[2, 2] = z;

        return scaleMatrix;
    }

    public static RayMatrix RotationX(float radians)
    {
        var rotateXMatrix = RayMatrix.Identity.CloneMatrix();
        rotateXMatrix[1, 1] = float.Cos(radians);
        rotateXMatrix[1, 2] = float.Sin(radians) * -1;
        rotateXMatrix[2, 1] = float.Sin(radians);
        rotateXMatrix[2, 2] = float.Cos(radians);

        return rotateXMatrix;
    }

    public static RayMatrix RotationY(float radians)
    {
        var rotateYMatrix = RayMatrix.Identity.CloneMatrix();
        rotateYMatrix[0, 0] = float.Cos(radians);
        rotateYMatrix[0, 2] = float.Sin(radians);
        rotateYMatrix[2, 0] = float.Sin(radians) * -1;
        rotateYMatrix[2, 2] = float.Cos(radians);

        return rotateYMatrix;
    }

    public static RayMatrix RotationZ(float radians)
    {
        var rotateYMatrix = RayMatrix.Identity.CloneMatrix();
        rotateYMatrix[0, 0] = float.Cos(radians);
        rotateYMatrix[0, 1] = float.Sin(radians) * -1;
        rotateYMatrix[1, 0] = float.Sin(radians);
        rotateYMatrix[1, 1] = float.Cos(radians);

        return rotateYMatrix;
    }

    public static RayMatrix Shearing(
          float xy
        , float xz
        , float yx
        , float yz
        , float zx
        , float zy
    )
    {
        var shearMatrix = RayMatrix.Identity.CloneMatrix();
        shearMatrix[0, 1] = xy;
        shearMatrix[0, 2] = xz;
        shearMatrix[1, 0] = yx;
        shearMatrix[1, 2] = yz;
        shearMatrix[2, 0] = zx;
        shearMatrix[2, 1] = zy;

        return shearMatrix;
    }
}
