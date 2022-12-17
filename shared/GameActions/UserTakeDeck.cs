using Shared.PossibleCards;

namespace Shared.GameActions;

public record UserTakeDeck : GameAction, IOneUserInfo
{
    public UserTakeDeck(int user, AllCards[] cards)
    {
        if (cards.Length != 5)
            throw new ArgumentException("Player must take 5 cards");
        UserId = user;
        CardsFromDeck = cards;
    }
    
    public UserTakeDeck() {}
    
    public AllCards[] CardsFromDeck { get; init; }
}