using Shared.PossibleCards;

namespace CardWarsClient.ClientLogic.Cards;

public class Land : Card
{
    public bool IsTurnedOut { get; private set; }
    
    public LandType LandType { get; }

    public Land(LandType land, string frontSuit, string backSuit) : base(frontSuit, backSuit)
    {
        if (LandType.any == land)
            throw new ArgumentException("Land can not be Any");
        LandType = land;
    }

    public void TurnOut() => IsTurnedOut = !IsTurnedOut;
}