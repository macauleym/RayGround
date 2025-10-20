namespace RayGround.Core.Interfaces.Entities;

public interface IPaintable<out T>
{
    T Paint(Material with);
}
