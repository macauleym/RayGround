namespace RayGround.Core.Interfaces;

public interface IMorphable<out T>
{
    T Morph(Matrix with);
}
