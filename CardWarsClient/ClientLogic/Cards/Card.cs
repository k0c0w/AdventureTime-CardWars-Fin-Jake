using Shared.PossibleCards;

namespace CardWarsClient.ClientLogic.Cards;

public abstract class Card
{
    public readonly string? FrontSuitPath;
    public readonly string BackSuitPath;
    
    public Card(string frontSuit, string backSuit)
    {
        if (string.IsNullOrEmpty(frontSuit) || string.IsNullOrEmpty(backSuit))
            throw new ArgumentNullException("Suits can not be null or empty");
        if (!File.Exists(frontSuit) || !File.Exists(backSuit))
            throw new ArgumentException("Suit not found");
        
        FrontSuitPath = frontSuit;
        BackSuitPath = backSuit;
    }

    //для карт соперника и колоды
    public Card(string backSuit)
    {
        if (string.IsNullOrEmpty(backSuit))
            throw new ArgumentNullException($"{nameof(backSuit)} can not be null or empty");
        if (!File.Exists(backSuit))
            throw new ArgumentException($"Suit not found: {backSuit}");
        
        BackSuitPath = backSuit;
    }
}