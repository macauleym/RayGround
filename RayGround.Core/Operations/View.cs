using RayGround.Core.Extensions;

namespace RayGround.Core.Operations;

public class View
{
    public static RayMatrix Transform(RayTuple from, RayTuple to, RayTuple up)
    {
        var forward = (to - from).Normalize();
        var upNormal = up.Normalize();
        var left = forward.Cross(upNormal);
        var trueUp = left.Cross(forward);

        var orientation = new RayMatrix(4, 4)
        {
            [0] = [ left.X     , left.Y     , left.Z     , 0 ],
            [1] = [ trueUp.X   , trueUp.Y   , trueUp.Z   , 0 ],
            [2] = [ -forward.X , -forward.Y , -forward.Z , 0 ],
            [3] = [          0 ,          0 ,          0 , 1 ]
        };

        return orientation * Operations.Transform.Translation(-from.X, -from.Y, -from.Z);
    }
}