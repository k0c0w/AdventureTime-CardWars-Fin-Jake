using Shared.Decks;
using Shared.PossibleCards;

namespace GameObjects;

public class Deck
{
    public DeckTypes DeckType { get; }

    public int CardsLeft => _cardsInDeck.Count;
    
    private  Queue<AllCards> _cardsInDeck;

    private Random _random = new Random();
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
        var cards = _cardsInDeck.ToArray();
        for (var n = cards.Length - 1; n > 0; --n)
        {
            var k = _random.Next(n+1);
            (cards[n], cards[k]) = (cards[k], cards[n]);
        }

        _cardsInDeck = new Queue<AllCards>(cards);
    }

    public void ReturnCardToDeck(AllCards card) => _cardsInDeck.Enqueue(card);
}