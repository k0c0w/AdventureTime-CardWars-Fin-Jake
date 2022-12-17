using Shared.PossibleCards;

namespace Shared.GameActions;

public record UserPutCard : CardAction, IBothUserInfo
{
    public UserPutCard(int userId, int line, AllCards card, int indexInHand = -1) : base(userId, line, card)
        => IndexInHand = indexInHand;
    
    public int IndexInHand { get;}
} 