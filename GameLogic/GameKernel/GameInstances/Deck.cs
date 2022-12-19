using Shared.Decks;
using Shared.PossibleCards;

namespace GameObjects;

public class Deck
{
    public DeckTypes DeckType { get; }

    public int CardsLeft => _cardsInDeck.Count;
    
    private readonly Queue<AllCards> _cardsInDeck;
    public Deck(DeckTypes deckType, Queue<AllCards> cards)
    {
        if (cards.Count != 40)
            throw new ArgumentException("Deck must consist of 40 cards");
        _cardsInDeck = cards;
        DeckType = deckType;
    }

    public bool IsEmpty => _cardsInDeck.Count == 0;

    public AllCards GetCard() => _cardsInDeck.Dequeue();

    public void Shuffle()
    {
        //todo:
        throw new NotImplementedException();
    }

    public void ReturnCardToDeck(AllCards card) => _cardsInDeck.Enqueue(card);
}