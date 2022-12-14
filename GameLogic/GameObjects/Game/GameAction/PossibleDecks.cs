using GameKernel.Deck;

namespace GameKernel;

public record PossibleDecks() : GameAction
{
    public DeckTypes First { get; init; }
    public DeckTypes Second { get; init; }
};