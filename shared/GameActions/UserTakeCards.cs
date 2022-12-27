using Shared.PossibleCards;

namespace Shared.GameActions;

public record UserTakeCards : GameAction, IOneUserInfo
{
    public int CardsInDeckLeft { get;}

    public int TakenCards => Cards.Length;
    public AllCards[] Cards { get; }

    public UserTakeCards(int userId, AllCards[] cards, int cardsInDeck)
    {
        UserId = userId;
        Cards = cards;
        CardsInDeckLeft = cardsInDeck;
    }
}