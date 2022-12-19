using Shared.PossibleCards;

namespace CardWarsClient.ClientLogic.Cards;

public class GameCard : Card
{
    public int TakenDamage { get; set; }
    
    public bool IsFlooped { get; set; }
    
    public AllCards Card { get; }
    
    public GameCard(AllCards card, string frontSuit, string backSuit) : base(frontSuit, backSuit)
    {
        Card = card;
    }
}