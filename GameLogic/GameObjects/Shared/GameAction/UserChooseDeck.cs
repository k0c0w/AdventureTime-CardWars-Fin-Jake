using GameKernel.Deck;

namespace GameKernel;

public record UserDeckChoose : GameAction
{
    public DeckTypes Deck { get; init; }

    public UserDeckChoose(int id, DeckTypes deck)
    {
        UserId = id;
        Deck = deck;
    }
}