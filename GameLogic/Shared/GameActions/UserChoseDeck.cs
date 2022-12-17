using Shared.Decks;

namespace Shared.GameActions;

public record class UserChoseDeck : GameAction, IBothUserInfo
{
    public DeckTypes DeckType { get; }
    
    public UserChoseDeck(int userId, DeckTypes deck)
    {
        DeckType = deck;
        UserId = userId;
    }
}