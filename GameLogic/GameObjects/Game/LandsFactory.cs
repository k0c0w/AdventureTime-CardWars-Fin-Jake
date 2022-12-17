using GameObjects;
using Shared.Decks;
using Shared.PossibleCards;

namespace GameKernel;

internal class LandsFactory
{
    public static DecksAndLandsHolder PrepareLandsAndDecks(FinnVSJake setting)
    {
        var finn = new Land[4]
        {
            new Land(LandType.BluePlains), new Land(LandType.BluePlains), 
            new Land(LandType.BluePlains), new Land(LandType.BluePlains)
        };
        
        var jake = new Land[4]
        {
            new Land(LandType.CornFields), new Land(LandType.CornFields), 
            new Land(LandType.CornFields), new Land(LandType.CornFields)
        };

        var finnDeck = CreateFinnDeck();
        var jakeDeck = CreateJakeDeck();
        return new DecksAndLandsHolder
        {
            First = Tuple.Create(finnDeck, finn),
            Second = Tuple.Create(jakeDeck, jake)
        };
    }

    private static Deck CreateFinnDeck()
    {
        var cards = new Queue<AllCards>(40);
        for (var i = 0; i < 40; i++)
            cards.Enqueue(AllCards.SpiritSolder);

        return new Deck(DeckTypes.FinnDeck, cards);
    }

    private static Deck CreateJakeDeck()
    {
        var cards = new Queue<AllCards>(40);
        for (var i = 0; i < 40; i++)
            cards.Enqueue(AllCards.CornRonin);

        return new Deck(DeckTypes.JakeDeck, cards);
    }
}