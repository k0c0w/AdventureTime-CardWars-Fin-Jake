namespace GameObjects;

public interface IFloopable
{
    bool CanBeFlooped();

    void Floop();

    bool IsFlooped();
}