using Shared.PossibleCards;

namespace Shared.GameActions;

public record UserTakeCard : GameAction, IOneUserInfo
{
    public int CardsInDeckLeft { get; init; }

    public AllCards Card { get; init; }

    public UserTakeCard(int userId, AllCards card, int cardsInDeck)
    {
        UserId = userId;
        Card = card;
        CardsInDeckLeft = cardsInDeck;
    }
}