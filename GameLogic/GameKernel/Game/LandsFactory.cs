﻿using GameObjects;
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
            cards.Enqueue(i % 7 == 2 ? AllCards.cool_dog : AllCards.spirit_solder);
        }

        return new Deck(DeckTypes.FinnDeck, cards).Shuffle();
    }

    private static Deck CreateJakeDeck()
    {
        var cards = new Queue<AllCards>(40);
        for (var i = 0; i < 40; i++)
            cards.Enqueue(AllCards.corn_ronin);

        return new Deck(DeckTypes.JakeDeck, cards).Shuffle();
    }
}