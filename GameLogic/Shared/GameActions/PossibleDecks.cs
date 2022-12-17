using Shared.Decks;

namespace Shared.GameActions;

public record PossibleDecks : GameAction, IBothUserInfo
{
    public DeckTypes First { get; init; }
    public DeckTypes Second { get; init; }
};