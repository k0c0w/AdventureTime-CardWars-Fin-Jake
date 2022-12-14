using GameKernel.Deck;

namespace GameKernel;

public class GameSetting
{
    public (DeckTypes, DeckTypes) Decks { get; }

    public GameSetting(DeckTypes first, DeckTypes second) => Decks = (first, second);
}

public class FinnVSJake : GameSetting
{
    public FinnVSJake() : base(DeckTypes.FinnDeck, DeckTypes.JakeDeck) {}
}