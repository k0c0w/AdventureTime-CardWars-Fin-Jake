using GameKernel.Deck;

namespace GameKernel;

public record class UserChoseDeck : GameAction
{
    public DeckTypes DeckType { get; }
    
    public UserChoseDeck(int userId, DeckTypes deck)
    {
        DeckType = deck;
        UserId = userId;
    }
}