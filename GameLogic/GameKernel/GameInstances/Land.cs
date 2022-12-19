using Shared.PossibleCards;

namespace GameObjects;

public class Land
{
    public bool IsTurnedOver { get; private set; }

    //todo: сообщить игре
    public void TurnOver() => IsTurnedOver = !IsTurnedOver;
    
    public LandType LandType { get; }

    public Land(LandType land)
    {
        if (LandType.any == land)
            throw new ArgumentException($"The {nameof(LandType)} must be assigned.");
        LandType = land;
    }
}