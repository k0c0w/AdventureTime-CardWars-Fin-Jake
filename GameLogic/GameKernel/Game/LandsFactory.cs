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
            new (LandType.blue_plains), new (LandType.blue_plains), 
            new (LandType.blue_plains), new (LandType.blue_plains)
        };
        
        var jake = new Land[4]
        {
            new (LandType.corn_fields), new (LandType.corn_fields), 
            new (LandType.corn_fields), new (LandType.corn_fields)
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
        {
            if (i % 10 == 4)
                cards.Enqueue(AllCards.cool_dog);
            else if(i % 13 == 3)
                cards.Enqueue(AllCards.spirit_solder);
            else
                cards.Enqueue(AllCards.nice_ice_baby);
        }

        return new Deck(DeckTypes.FinnDeck, cards).Shuffle();
    }

    private static Deck CreateJakeDeck()
    {
        var cards = new Queue<AllCards>(40);
        for (var i = 0; i < 40; i++)
        {
            if (i % 10 == 4)
                cards.Enqueue(AllCards.corn_ronin);
            else if(i % 13 == 3)
                cards.Enqueue(AllCards.corn_dog);
            else if(i % 5 == 0)
                cards.Enqueue(AllCards.corn_knight);
            else if(i % 7 == 0)
                cards.Enqueue(AllCards.king_of_fields);
            else
                cards.Enqueue(AllCards.corn_wall);
        }

        return new Deck(DeckTypes.JakeDeck, cards).Shuffle();
    }
}