using Shared.PossibleCards;

namespace Shared.GameActions;

public record UserTakeDeck : GameAction, IOneUserInfo
{
    public UserTakeDeck(int user, AllCards[] cards, LandType[] lands)
    {
        if (cards.Length != 5)
            throw new ArgumentException("Player must take 5 cards");
        if (lands.Length != 4 || lands.Contains(LandType.Any))
            throw new ArgumentException("Invalid Lands");
        UserId = user;
        CardsFromDeck = cards;
        Lands = lands;
    }
    
    public AllCards[] CardsFromDeck { get;}
    
    public LandType[] Lands { get; }
}