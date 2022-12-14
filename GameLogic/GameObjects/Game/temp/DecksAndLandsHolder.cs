using GameKernel.Deck;
using GameObjects;

namespace GameKernel.temp;

public class DecksAndLandsHolder
{
    public Tuple<Deck, Land[]>? First { get; set; }
    
    public Tuple<Deck, Land[]>? Second { get; set; }

    public bool ContainsDeck(DeckTypes deck)
        => First != null && First.Item1.DeckType == deck || Second != null && Second.Item1.DeckType == deck;

    public Tuple<Deck, Land[]> GrabDeck(DeckTypes deck)
    {
        if (First != null && First.Item1.DeckType == deck)
        {
            var item = First;
            First = null;
            return item;
        }
        else if( Second != null && Second.Item1.DeckType == deck)
        {
            var item = Second;
            Second = null;
            return item;
        }

        throw new InvalidOperationException($"No such deck {deck} left!");
    }
}

public class Deck
{
    public DeckTypes DeckType;
    
    public void Shuffle() {}
}