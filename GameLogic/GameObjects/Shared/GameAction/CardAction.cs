using GameObjects.Shared.Enums;

namespace GameKernel;

public record CardAction : GameAction
{
    public AllCards Card { get; init; }

    public int Line { get; init; }
    
    public CardAction() {}

    public CardAction(int userId, int cardIndex, AllCards card)
    {
        UserId = userId;
        Card = card;
        Line = cardIndex;
    }
}