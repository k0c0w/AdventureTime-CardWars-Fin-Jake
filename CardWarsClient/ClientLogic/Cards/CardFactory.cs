using Shared.PossibleCards;

namespace CardWarsClient.ClientLogic.Cards;

public class CardFactory
{
    private static Random random = new Random();
    
    private static string BacksideLand = @"./Resources/Images/Cards/Lands/reversed_land.png";

    private static string BacksideCard = @"./Resources/Images/Cards/backside.png";

    public static Land CreateLand(LandType land)
    {
        //CreateLand(land, $"./Resources/Images/Cards/Lands/{land}_{random.Next(1, 4)}.png");
        return CreateLand(land, $"./Resources/Images/Cards/Lands/{land}.png");
    }

    public static Card CreateCard(AllCards card)
        => new GameCard(card, $"./Resources/Images/Cards/GameCards/{card}.png", BacksideCard);

    public static Card CreateBackSuitGameCard() => new GameCardBackOnly(BacksideCard);

    private static Land CreateLand(LandType land, string frontPath) => new Land(land, frontPath, BacksideLand);
}