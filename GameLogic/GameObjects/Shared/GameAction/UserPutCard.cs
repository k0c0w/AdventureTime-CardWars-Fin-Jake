using GameObjects;
using GameObjects.Shared.Enums;
using AllCards = GameObjects.Shared.Enums.AllCards;

namespace GameKernel;

public record UserPutCard : CardAction, IBothUserInfo
{
    public UserPutCard(int userId, int line, AllCards card, int? indexInHand = null) : base(userId, line, card)
        => IndexInHand = indexInHand;
    
    public int? IndexInHand { get; init; } = null;
} 