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
            new Land(LandType.blue_plains), new Land(LandType.blue_plains), 
            new Land(LandType.blue_plains), new Land(LandType.blue_plains)
        };
        
        var jake = new Land[4]
        {
            new Land(LandType.corn_fields), new Land(LandType.corn_fields), 
            new Land(LandType.corn_fields), new Land(LandType.corn_fields)
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
            cards.Enqueue(AllCards.spirit_solder);

        return new Deck(DeckTypes.FinnDeck, cards);
    }

    private static Deck CreateJakeDeck()
    {
        var cards = new Queue<AllCards>(40);
        for (var i = 0; i < 40; i++)
            cards.Enqueue(AllCards.corn_ronin);

        return new Deck(DeckTypes.JakeDeck, cards);
    }
}